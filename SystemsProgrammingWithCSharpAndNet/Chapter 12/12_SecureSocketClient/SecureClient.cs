using System.Net.Security;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ExtensionLibrary;

namespace _12_SecureSocketClient;

internal class SecureClient
{
    private readonly int _port;
    private readonly string _server;

    public SecureClient(string server, int port)
    {
        _server = server;
        _port = port;
    }

    public async Task ConnectAsync()
    {
        using var clientSocket = new TcpClient(_server, _port);
        await using var networkStream = clientSocket.GetStream();
        await using var sslStream = new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate));
        try
        {
            await sslStream.AuthenticateAsClientAsync(_server);
            "SSL authentication successful".Dump();
            
            string message = $"Hello, server! {DateTime.Now.TimeOfDay}";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            
            await sslStream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
        catch (Exception ex)
        {
            ex.Message.Dump(ConsoleColor.Red);
        }

    }

    private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            "Server certificate is valid".Dump();
            return true;
        }
        
        "Server certificate is invalid".Dump(ConsoleColor.Red);
        return false;
    }
}