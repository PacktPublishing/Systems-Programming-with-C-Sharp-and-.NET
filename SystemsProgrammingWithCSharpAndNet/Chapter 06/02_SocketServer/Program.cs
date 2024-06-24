using ExtensionLibrary;

using System.Net;
using System.Net.Sockets;
using System.Text;

"Server is starting up.".Dump();

var server = new TcpListener(IPAddress.Loopback, 8080);
server.Start();

"Waiting for a connection.".Dump();

var client = await server.AcceptTcpClientAsync();
"Client connected".Dump();

var stream = client.GetStream();
while (true)
{
    var buffer = new byte[1024];
    var bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
    var message = Encoding.UTF8.GetString(buffer, 0, bytes);
    $"Received message: {message}".Dump();

    if (message.ToLower() == "bye")
        break;

    "Say something back".Dump();
    var response = Console.ReadLine();
    var responseBytes = Encoding.UTF8.GetBytes(response);
    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

    if (response.ToLower() == "bye")
        break;
}

client.Close();
server.Stop();
"Connection closed.".Dump();