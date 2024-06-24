// See https://aka.ms/new-console-template for more information

using ExtensionLibrary;

using System.IO.Pipes;

await using var client = new NamedPipeClientStream(".", "SystemsProgrammersPipe");
"Connecting to the server".Dump(ConsoleColor.Yellow);
await client.ConnectAsync();
using var reader = new StreamReader(client);
string? message = await reader.ReadLineAsync();
message.Dump(ConsoleColor.Yellow);


