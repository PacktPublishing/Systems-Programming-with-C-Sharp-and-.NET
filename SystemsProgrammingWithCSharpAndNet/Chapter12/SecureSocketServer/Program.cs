using ExtensionLibrary;
using SecureSocketServer;

"Server is starting...".Dump();
// Replace this with your actual path to your actual PFX file
var certificatePath = @"d:\Certificate\testcer.pfx";
var certificatePassword = "password";

var server = new SecureServer(
    8081,
    certificatePath,
    certificatePassword);

await server.StartAsync();