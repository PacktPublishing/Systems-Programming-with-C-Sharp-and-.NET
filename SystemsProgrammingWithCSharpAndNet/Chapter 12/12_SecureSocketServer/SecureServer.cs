using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ExtensionLibrary;

namespace _12_SecureSocketServer;

internal class SecureServer
{
    private readonly X509Certificate2 _serverCertificate;

    private readonly int _port;

    public SecureServer(int port,
        string certificatePath,
        string certificatePassword)
    {
        _port = port;
        _serverCertificate = new X509Certificate2(
            certificatePath,
            certificatePassword);
    }

    public async Task StartAsync()
    {
        "Server is starting...".Dump();
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();
        $"Server is listening on port {_port}...".Dump();

        while (true)
        {
            var clientSocket = await listener.AcceptSocketAsync();
            _ = HandleClientConnection(clientSocket);
        }
    }

    private async Task HandleClientConnection(Socket clientSocket)
    {
        try
        {
            await using var sslStream = 
                new SslStream(
                    new NetworkStream(clientSocket), 
                    false);

            await sslStream.AuthenticateAsServerAsync(
                _serverCertificate, 
                false,
                SslProtocols.Tls12, 
                true);
            $"Client connected: {clientSocket.RemoteEndPoint}".Dump();

            var buffer = new byte[1024];
            var bytesRead = 
                await sslStream.ReadAsync(
                    buffer, 
                    0, 
                    buffer.Length);

            var receivedString = 
                Encoding.UTF8.GetString(
                    buffer, 
                    0, 
                    bytesRead);
            $"Received from client: {receivedString}".Dump();
        }
        catch (Exception ex)
        {
            ex.Message.Dump();
        }
    }
}