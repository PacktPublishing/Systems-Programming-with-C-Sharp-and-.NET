using ExtensionLibrary;
using System.IO.MemoryMappedFiles;

"Wait for the server to finish. \nPress Enter to read the shared data.".Dump(ConsoleColor.Yellow);
Console.ReadLine();

using var mmf = MemoryMappedFile.OpenExisting("SharedData");
// Create a view accessor to read data
using MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();
byte[] data = new byte[1024];
accessor.ReadArray(0, data, 0, data.Length);

$"Received message: {System.Text.Encoding.UTF8.GetString(data)}".Dump(ConsoleColor.Yellow);