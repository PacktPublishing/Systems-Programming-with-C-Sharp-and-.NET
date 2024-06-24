using ExtensionLibrary;
using System.Diagnostics;
using System.IO.Pipes;

await using var pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
$"The pipe handle is: {pipeServer.GetClientHandleAsString()}".Dump(ConsoleColor.Cyan);

pipeServer.DisposeLocalCopyOfClientHandle();

await using var sw = new StreamWriter(pipeServer);
sw.AutoFlush = true;
sw.WriteLine("From server");
pipeServer.WaitForPipeDrain();


Process pipeClient = new Process();

pipeClient.StartInfo.FileName = @"pipeClient.exe";

// Pass the client process a handle to the server.
pipeClient.StartInfo.Arguments =
    pipeServer.GetClientHandleAsString();
pipeClient.StartInfo.UseShellExecute = true;
pipeClient.Start();
