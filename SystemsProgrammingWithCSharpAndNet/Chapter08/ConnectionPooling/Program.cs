using ConnectionPooling;

await using var connectionPool = new TcpClientConnectionPool();

var myConnection = connectionPool.GetConnection();
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