using System.IO.Pipes;
using ExtensionLibrary;

namespace AnonymousPipeClient;

internal class Program
{
    public static void Main(string[] args)
    {
        "Enter the pipeHandle".Dump(ConsoleColor.Yellow);
        var pipeHandle = Console.ReadLine();

        using var pipeClient = new AnonymousPipeClientStream(PipeDirection.In, pipeHandle);
        using var sr = new StreamReader(pipeClient);
        while (sr.ReadLine() is { } temp)
            temp?.Dump(ConsoleColor.Yellow);
    }
}