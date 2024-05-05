// See https://aka.ms/new-console-template for more information

using ExtensionLibrary;

Console.WriteLine("Hello, World!");


for (int i = 0; i < 10; i++)
{
    ThreadPool.QueueUserWorkItem(CallBack);
}

"Main thread".Dump(ConsoleColor.Cyan);
byte[] data;
while (true)
{
    $"In the main thread {Thread.CurrentThread.ManagedThreadId} as {DateTime.Now}".Dump(ConsoleColor.Cyan);

    data = new byte[10000000];
    await Task.Run(async () =>
    {
        await Task.Delay(500);
        

    });
    await Task.Delay(500);
}

return;


void CallBack(object? _)
{
    int internalCounter = 0;
    while (true)
    {
        $"In the thread {Thread.CurrentThread.ManagedThreadId} as {DateTime.Now}\nWith Counter {internalCounter++}".Dump(ConsoleColor.Yellow);
        Thread.Sleep(500);
    }
}
