using MelonLoader;
using UnityEngine;
using Il2CppScheduleOne.Casino;
using Harmony;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Il2CppFishNet.Object;
using Il2CppScheduleOne.Casino.UI;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace ScheduleIMod
{
    public static class BuildInfo
    {
        public const string Name = "ScheduleIMod";
        public const string Description = "Schedule 1 SlotMachine Rigger";
        public const string Author = "notfishvr";
        public const string Company = null;
        public const string Version = "1.1.0";
        public const string DownloadLink = null;
    }

    public class ScheduleIModGUI : MelonMod
    {
        private Rect windowRect = new Rect(45, 65, 350, 465);
        private bool showGUI = false;
        private SlotMachine.EOutcome selectedOutcome = SlotMachine.EOutcome.Jackpot;
        private string[] outcomeNames;
        private int selectedOutcomeIndex = 0;

        public override void OnInitializeMelon()
        {
            try
            {
                outcomeNames = System.Enum.GetNames(typeof(SlotMachine.EOutcome));
                MelonLogger.Msg("ScheduleIModGUI Initialized");
            }
            catch (System.Exception e)
            {
                MelonLogger.Error($"Error in ScheduleIModGUI Initialization: {e.Message}");
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F4))
            {
                showGUI = !showGUI;
                MelonLogger.Msg($"F4 Pressed. GUI Visibility: {showGUI}");
            }
        }

        public override void OnGUI()
        {
            try
            {
                if (showGUI)
                {
                    System.Action<int> value = id => DrawWindow(id);
                    windowRect = GUI.Window(0, windowRect, value, "Schedule I Mod GUI");

                    GUI.DragWindow(new Rect(windowRect));
                }
            }
            catch (System.Exception e)
            {
                MelonLogger.Error($"Error in OnGUI: {e.Message}");
            }
        }
        private void DrawWindow(int windowID)
        {
            try
            {
                float y = 20f;

                GUI.Label(new Rect(10, y, 230, 20), "Select Forced Outcome:");
                y += 20f;

                for (int i = 0; i < outcomeNames.Length; i++)
                {
                    bool isSelected = (selectedOutcomeIndex == i);
                    if (GUI.Toggle(new Rect(10, y, 230, 20), isSelected, outcomeNames[i]))
                    {
                        selectedOutcomeIndex = i;
                    }
                    y += 25f;
                }

                if (GUI.Button(new Rect(10, y, 230, 20), "Apply Forced Outcome"))
                {
                    if (System.Enum.TryParse<SlotMachine.EOutcome>(outcomeNames[selectedOutcomeIndex], out var selectedOutcome))
                    {
                        ScheduleIMod.SetRigging_SlotMachine(true, selectedOutcome);
                        MelonLogger.Msg($"Forced Outcome: {selectedOutcome}");
                    }
                    else
                    {
                        MelonLogger.Error("Failed to parse forced outcome.");
                    }
                }
                y += 30f;

                if (GUI.Button(new Rect(10, y, 230, 20), "Log Cards"))
                {
                    PlayingCard[] allCards = Object.FindObjectsOfType<PlayingCard>();

                    foreach (PlayingCard card in allCards)
                    {
                        LogCardDetails(card);   
                    }
                }

                y += 30f;


                GUI.Label(new Rect(10, y, 230, 20), "Hotkeys:");
                y += 20f;
                GUI.Label(new Rect(10, y, 230, 20), "F4: Toggle GUI");

                GUI.DragWindow();
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"Error in DrawWindow: {ex.Message}");
            }
        }
        private void LogCardDetails(PlayingCard card)
        {
            MelonLogger.Msg($"Card Details - " +
                $"ID: {card.CardID}, " +
                $"Suit: {card.Suit}, " +
                $"Value: {card.Value}, " +
                $"Face Up: {card.IsFaceUp}");

            //card.CardController.cardDictionary
            //card.CardController.cards
        }
    }

    public class ScheduleIMod : MelonMod
    {
        private static bool isRiggingEnabled_SlotMachine = false;

        private static bool forceBlackjack = true;
        private static bool givePlayerAce = true;

        private static SlotMachine.EOutcome forcedOutcome = SlotMachine.EOutcome.Jackpot;
        private static SlotMachine.ESymbol[] forcedSymbols = null;
        public ScheduleIModGUI scheduleIModGUI = new ScheduleIModGUI();

        public static bool enableExtraCardMods = true;
        public static PlayingCard.ECardValue forcedCardValue = PlayingCard.ECardValue.Ace;
        public static PlayingCard.ECardSuit forcedCardSuit = PlayingCard.ECardSuit.Hearts;

        public static bool IsRiggingEnabled_SlotMachine
        {
            get { return isRiggingEnabled_SlotMachine; }
            private set { isRiggingEnabled_SlotMachine = value; }
        }

        public override void OnUpdate()
        {
            scheduleIModGUI.OnUpdate();
        }

        [System.Obsolete]
        public override void OnInitializeMelon()
        {
            this.scheduleIModGUI = new ScheduleIModGUI();
            scheduleIModGUI.OnInitializeMelon();

            HarmonyInstance harmony = new HarmonyInstance(this.GetType().Namespace);
            harmony.Patch(
                typeof(SlotMachine).GetMethod(nameof(SlotMachine.GetRandomSymbol)),
                postfix: new Harmony.HarmonyMethod(typeof(ScheduleIMod).GetMethod(nameof(GetRandomSymbolPatch_SlotMachine)))
            );
            harmony.Patch(
                typeof(SlotMachine).GetMethod("EvaluateOutcome", BindingFlags.NonPublic ),
                prefix: new Harmony.HarmonyMethod(typeof(ScheduleIMod).GetMethod(nameof(EvaluateOutcomePatch_SlotMachine)))
            );

            MelonLogger.Msg("Initialized!");
        }

        public override void OnGUI()
        {
            scheduleIModGUI.OnGUI();
        }

        public static void SetRigging_SlotMachine(bool enabled, SlotMachine.EOutcome outcome = SlotMachine.EOutcome.Jackpot)
        {
            isRiggingEnabled_SlotMachine = enabled;
            forcedOutcome = outcome;
            switch (outcome)
            {
                case SlotMachine.EOutcome.Jackpot:
                    forcedSymbols = new SlotMachine.ESymbol[] {
                        SlotMachine.ESymbol.Seven,
                        SlotMachine.ESymbol.Seven,
                        SlotMachine.ESymbol.Seven
                    };
                    break;
                case SlotMachine.EOutcome.BigWin:
                    forcedSymbols = new SlotMachine.ESymbol[] {
                        SlotMachine.ESymbol.Bell,
                        SlotMachine.ESymbol.Bell,
                        SlotMachine.ESymbol.Bell
                    };
                    break;
                default:
                    forcedSymbols = null;
                    break;
            }
        }
        public static void GetRandomSymbolPatch_SlotMachine(ref SlotMachine.ESymbol __result)
        {
            if (isRiggingEnabled_SlotMachine && forcedSymbols != null)
            {
                __result = forcedSymbols[UnityEngine.Random.Range(0, forcedSymbols.Length)];
            }
        }
        public static bool EvaluateOutcomePatch_SlotMachine(SlotMachine.ESymbol[] outcome, ref SlotMachine.EOutcome __result)
        {
            if (isRiggingEnabled_SlotMachine)
            {
                __result = forcedOutcome;
                return false;
            }
            return true;
        }
    }
}