using System.IO.Pipes;
using System.Text.Json;
using _02_JSON_RPC.Commands;
using ExtensionLibrary;

namespace _02_JSON_RPC;

internal class Client(CancellationToken cancellationToken) 
{
    public async Task StartClient()
    {

        var newCommand = new ShowDateCommand
        {
            IncludeTime = true
        };
        var newCommandAsJson = JsonSerializer.Serialize(newCommand);

        "Starting the client".Dump(ConsoleColor.Yellow);
        await using var client = new NamedPipeClientStream("CommandsPipe");
        await client.ConnectAsync(cancellationToken);
        await using var writer = new StreamWriter(client);
        $"Sending this command: {newCommandAsJson}".Dump(ConsoleColor.Yellow);
        await writer.WriteLineAsync(newCommandAsJson);
        await writer.FlushAsync();
    }
}