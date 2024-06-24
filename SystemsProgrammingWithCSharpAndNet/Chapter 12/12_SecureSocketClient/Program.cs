using System.Net.Sockets;
using System.Text;
using _12_SecureSocketClient;
using ExtensionLibrary;

"Client is starting...".Dump();
    "Press any key to connect to the server...".Dump();
    Console.ReadKey();

var secureClient = new SecureClient("localhost", 8081);
await secureClient.ConnectAsync();

