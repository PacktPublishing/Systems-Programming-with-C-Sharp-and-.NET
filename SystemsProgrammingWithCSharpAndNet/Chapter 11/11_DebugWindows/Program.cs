using ExtensionLibrary;


MyClass myClass = new MyClass();

int anotherCounter = 0;
for (int i = 0; i < 5; i++)
{
    ThreadPool.QueueUserWorkItem(CallBack);
    ThreadPool.QueueUserWorkItem(CallBackB);
}
int myNumber = 0;
while (true)
{
    myClass.Counter++;
    Console.WriteLine($"Counter {myClass.Counter++}");
    await Task.Delay(100);
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
    var rnd =  new Random();
    var randomDelay = rnd.Next(100, 1000);
    int internalCounter = 0;
    while (true)
    {
        
        $"In the thread {Thread.CurrentThread.ManagedThreadId} as {DateTime.Now}\nWith Counter {internalCounter++}".Dump(ConsoleColor.Yellow);
        myClass.Counter += rnd.Next(1, 5);
        Thread.Sleep(randomDelay);
    }
    
}

void CallBackB(object? _)
{
    var rnd = new Random();
    var randomDelay = rnd.Next(100, 1000);

    int internalCounter = 0;
    while (true)
    {
        $"In the thread B {Thread.CurrentThread.ManagedThreadId} as {DateTime.Now}\nWith Counter {internalCounter++}".Dump(ConsoleColor.Yellow);
        Thread.Sleep(randomDelay);
    }
}

internal class MyClass
{
    public int Counter { get; set; }
}