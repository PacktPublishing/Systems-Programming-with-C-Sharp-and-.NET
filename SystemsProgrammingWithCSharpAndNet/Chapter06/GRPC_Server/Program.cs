using ExtensionLibrary;
using Grpc.Core;
using GRPC_Server;

"Starting gRPC server...".Dump();
var port = 50051;

var server = new Server
{
    Services = {TimeDisplayer.BindService(new TimeDisplayerService())},
    Ports = {new ServerPort("localhost", port, ServerCredentials.Insecure)}
};
server.Start();

Console.WriteLine("Greeter server listening on port " + port);
Console.WriteLine("Press any key to stop the server...");
Console.ReadKey();

await server.ShutdownAsync();