using System.Diagnostics;
using System.IO.Pipes;
using ExtensionLibrary;

await using var pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
$"The pipe handle is: {pipeServer.GetClientHandleAsString()}".Dump();

//pipeServer.DisposeLocalCopyOfClientHandle();


var pipeClient = new Process();

pipeClient.StartInfo.FileName = @"C:\temp\PipeClient\Debug\net8.0\AnonymousPipeClient.exe";

// Pass the client process a handle to the server.
pipeClient.StartInfo.Arguments =
    pipeServer.GetClientHandleAsString();
pipeClient.StartInfo.UseShellExecute = false;
pipeClient.Start();

await using var sw = new StreamWriter(pipeServer);
sw.AutoFlush = true;
sw.WriteLine("From server");
pipeServer.WaitForPipeDrain();