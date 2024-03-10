// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using _08_ConnectionPooling;

await using var connectionPool = new TcpClientConnectionPool();

TcpClient? myConnection = connectionPool.GetConnection();
try
{
    var myBuffer = "Hello, World!"u8.ToArray();
    // Use the connection
    await myConnection.Client.SendAsync(myBuffer);
}
finally
{
    connectionPool.ReturnConnection(myConnection);
}
