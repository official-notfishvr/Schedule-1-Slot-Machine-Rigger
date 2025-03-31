using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(ScheduleIMod.BuildInfo.Description)]
[assembly: AssemblyDescription(ScheduleIMod.BuildInfo.Description)]
[assembly: AssemblyCompany(ScheduleIMod.BuildInfo.Company)]
[assembly: AssemblyProduct(ScheduleIMod.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + ScheduleIMod.BuildInfo.Author)]
[assembly: AssemblyTrademark(ScheduleIMod.BuildInfo.Company)]
[assembly: AssemblyVersion(ScheduleIMod.BuildInfo.Version)]
[assembly: AssemblyFileVersion(ScheduleIMod.BuildInfo.Version)]
[assembly: MelonInfo(typeof(ScheduleIMod.ScheduleIMod), ScheduleIMod.BuildInfo.Name, ScheduleIMod.BuildInfo.Version, ScheduleIMod.BuildInfo.Author, ScheduleIMod.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]