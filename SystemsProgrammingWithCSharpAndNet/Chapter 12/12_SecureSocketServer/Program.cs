using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using _12_SecureSocketServer;
using ExtensionLibrary;

"Server is starting...".Dump();
var certificatePath = @"c:\temp\MyServerCert.pfx";
var certificatePassword = "Welcome@1";

var server = new SecureServer(8081, certificatePath, certificatePassword);
await server.StartAsync();

