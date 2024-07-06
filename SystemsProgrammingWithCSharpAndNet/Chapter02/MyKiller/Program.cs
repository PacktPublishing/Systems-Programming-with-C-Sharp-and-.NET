using ExtensionLibrary;
using MyKiller;

"Press enter to kill the other app".Dump();

Console.ReadLine();
WindowFinder.KillWindow();
"Verify if it is dead.".Dump();

"We are done".Dump();