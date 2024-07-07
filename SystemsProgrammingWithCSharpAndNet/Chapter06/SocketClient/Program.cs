using ExtensionLibrary;
using System.Net.Sockets;
using System.Text;

"Client is starting up.".Dump(ConsoleColor.Yellow);

var client = new TcpClient("127.0.0.1", 8080);
"Connected to the server. Let's chat!".Dump(ConsoleColor.Yellow);
var stream = client.GetStream();

while (true)
{
    "Say something".Dump(ConsoleColor.Yellow);
    var message = Console.ReadLine();
    var data = Encoding.UTF8.GetBytes(message);
    await stream.WriteAsync(data, 0, data.Length);
    if (message.ToLower() == "bye")
        break;

    var buffer = new byte[1024];
    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
    var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    $"Server says: {response}".Dump(ConsoleColor.Yellow);
    if (response.ToLower() == "bye")
        break;
}

client.Close();
"Connection closed.".Dump(ConsoleColor.Yellow);