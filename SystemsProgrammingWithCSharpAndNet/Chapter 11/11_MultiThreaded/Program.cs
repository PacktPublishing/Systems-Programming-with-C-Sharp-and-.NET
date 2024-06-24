using _11_MultiThreaded;
using ExtensionLibrary;

Deadlock dl = new Deadlock();
dl.Run();

//VariableCheck vc = new VariableCheck();
//vc.Run();

"Waiting...".Dump(ConsoleColor.Cyan);
Console.ReadLine();