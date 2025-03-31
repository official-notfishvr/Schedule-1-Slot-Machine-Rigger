using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(Schedule1Mod.BuildInfo.Description)]
[assembly: AssemblyDescription(Schedule1Mod.BuildInfo.Description)]
[assembly: AssemblyCompany(Schedule1Mod.BuildInfo.Company)]
[assembly: AssemblyProduct(Schedule1Mod.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + Schedule1Mod.BuildInfo.Author)]
[assembly: AssemblyTrademark(Schedule1Mod.BuildInfo.Company)]
[assembly: AssemblyVersion(Schedule1Mod.BuildInfo.Version)]
[assembly: AssemblyFileVersion(Schedule1Mod.BuildInfo.Version)]
[assembly: MelonInfo(typeof(Schedule1Mod.Schedule1Mod), Schedule1Mod.BuildInfo.Name, Schedule1Mod.BuildInfo.Version, Schedule1Mod.BuildInfo.Author, Schedule1Mod.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]