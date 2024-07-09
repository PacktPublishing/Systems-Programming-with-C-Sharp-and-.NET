using System.IO.Pipes;
using ExtensionLibrary;

namespace AnonymousPipeClient;

internal class Program
{
    public static void Main(string[] args)
    {
        //"Enter the pipeHandle".Dump(ConsoleColor.Yellow);
        var pipeHandle = args[0];
        $"Pipehandle received = {pipeHandle}".Dump(ConsoleColor.Yellow);

        using var pipeClient = new AnonymousPipeClientStream(PipeDirection.In, pipeHandle);
        using var sr = new StreamReader(pipeClient);
        while (sr.ReadLine() is { } temp)
            temp?.Dump(ConsoleColor.Yellow);
    }
}