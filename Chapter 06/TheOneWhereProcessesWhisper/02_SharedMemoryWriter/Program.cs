using ExtensionLibrary;
using System.IO.MemoryMappedFiles;

"Ready to write data to share memory.\nPress Enter to do so.".Dump(ConsoleColor.Cyan);
Console.ReadLine();

using var mmf = MemoryMappedFile.CreateNew("SharedData", 1024);
// Create a view accessor to write data
using MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();
byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello from Process 1");
accessor.WriteArray(0, data, 0, data.Length);

"Data written to shared memory. Press any key to exit.".Dump(ConsoleColor.Cyan);
Console.ReadKey();