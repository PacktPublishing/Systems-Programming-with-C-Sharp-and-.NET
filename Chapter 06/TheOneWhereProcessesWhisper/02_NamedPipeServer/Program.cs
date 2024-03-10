// See https://aka.ms/new-console-template for more information

using ExtensionLibrary;

using System.IO.Pipes;

"Starting the server".Dump(ConsoleColor.Cyan);
await using var server = new NamedPipeServerStream("SystemsProgrammersPipe");
"Waiting for connection".Dump(ConsoleColor.Cyan);
await server.WaitForConnectionAsync();
await using var writer = new StreamWriter(server);
writer.AutoFlush = true;
writer.WriteLine("Hello from the server!");

Console.ReadLine();