using ExtensionLibrary;
using SecureSocketClient;

"Client is starting...".Dump();
"Press any key to connect to the server...".Dump();
Console.ReadKey();

var secureClient = new SecureClient("localhost", 8081);
await secureClient.ConnectAsync();