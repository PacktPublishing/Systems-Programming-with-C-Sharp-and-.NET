using System.IO.Pipes;
using System.Text.Json;
using ExtensionLibrary;
using JSON_RPC.Commands;

namespace JSON_RPC;

internal class Server(CancellationToken cancellationToken)
{
    public async Task StartServer()
    {
        "Starting the server".Dump(ConsoleColor.Cyan);
        await using var server = new NamedPipeServerStream("CommandsPipe");
        "Waiting for connection".Dump(ConsoleColor.Cyan);
        await server.WaitForConnectionAsync(cancellationToken);
        using var reader = new StreamReader(server);

        while (!cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync();
            if (line == null) break;
            $"Received this command: {line}".Dump(ConsoleColor.Cyan);

            var command = JsonSerializer.Deserialize<ShowDateCommand>(line);
            if (command is { IncludeTime: true })
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Dump(ConsoleColor.Cyan);
            else
                DateTime.Now.ToString("yyyy-MM-dd").Dump(ConsoleColor.Cyan);
        }
    }
}