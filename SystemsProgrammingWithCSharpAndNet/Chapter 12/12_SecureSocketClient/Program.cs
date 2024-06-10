using System.Net.Sockets;
using System.Text;
using _12_SecureSocketClient;
using ExtensionLibrary;

"Client is starting...".Dump();
    "Press any key to connect to the server...".Dump();
    Console.ReadKey();

var secureClient = new SecureClient("localhost", 8081);
await secureClient.ConnectAsync();

//while (true)
//{
//    "Press any key to connect to the server...".Dump();
//    Console.ReadKey();

//    try
//    {
//        // Create a TCP/IP socket
//        var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

//        // Connect to the server
//        clientSocket.Connect("localhost", 8081);

//        // Send a string to the server
//        var message = $"Hello, server! {DateTime.Now.TimeOfDay}";
//        var buffer = Encoding.UTF8.GetBytes(message);
//        clientSocket.Send(buffer);

//        // Close the socket
//        clientSocket.Close();
//    }
//    catch (Exception ex)
//    {
//        ex.Message.Dump();
//    }
//}