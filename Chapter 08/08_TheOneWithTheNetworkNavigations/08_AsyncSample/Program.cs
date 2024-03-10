// See https://aka.ms/new-console-template for more information

using _08_AsyncSample;
using ExtensionLibrary;

"Getting the current time...".Dump(ConsoleColor.Cyan);

var timeReader = new TimeReader();
"Reading the time synchronously...".Dump(ConsoleColor.Yellow);
var syncTime = timeReader.SyncGetNetworkTime();
$"Time retrieved synchronously: {syncTime.TimeOfDay:g}".Dump(ConsoleColor.Green);

"Reading the time asynchronously...".Dump(ConsoleColor.Yellow);
var asyncTime = await timeReader.ProperGetNetworkTimeAsync();
$"Time retrieved asynchronously: {asyncTime.TimeOfDay:g}".Dump(ConsoleColor.Green);


"Done.".Dump(ConsoleColor.Cyan);