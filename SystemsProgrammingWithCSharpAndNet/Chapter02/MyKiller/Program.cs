// See https://aka.ms/new-console-template for more information
using MyKiller;

Console.WriteLine("Press enter to kill the other app");

Console.ReadLine();
WindowFinder.KillWindow();
Console.WriteLine("Verify if it is dead.");

Console.WriteLine("We are done");