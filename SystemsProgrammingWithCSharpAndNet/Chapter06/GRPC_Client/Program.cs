using ExtensionLibrary;
using Grpc.Net.Client;
using GRPC_Server;

"Starting gRPC client... Press ENTER to connect.".Dump(ConsoleColor.Yellow);
Console.ReadLine();
var channel = GrpcChannel.ForAddress("http://localhost:50051");
var client = new TimeDisplayer.TimeDisplayerClient(channel);
var reply =
    await client.DisplayTimeAsync(
        new DisplayTimeRequest
        {
            Name = "World",
            WantsTime = false
        });
Console.WriteLine("From server: " + reply.Message);