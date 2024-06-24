using ExtensionLibrary;
using System.Collections.Concurrent;

// We have a collection that blocks as soon as 
// 5 items have been added. Before this thread
// can continue, one has to be taken away first.
var allLines = new BlockingCollection<string>(boundedCapacity:5);
ThreadPool.QueueUserWorkItem((_) => {
    for (int i = 0; i < 10; i++)
    {
        allLines.Add($"Line {i:000}");
        Thread.Sleep(1000);
    }
    allLines.CompleteAdding();
});

// Give the first thread some time to add items before 
// we take them away again.
Thread.Sleep(6000);

// Read all items by taking them away
ThreadPool.QueueUserWorkItem((_) => {
    while (!allLines.IsCompleted)
    {
        try
        {
            var item = allLines.Take();
            item.Dump(ConsoleColor.Yellow);
            Thread.Sleep(10);
        }
        catch (InvalidOperationException)
        {
            // This can happen if
            // CompleteAdding has been called
            // but the collection is already empty
            // in our case: this thread finished before the first one.
        }
    }
});


"Main app is done.\nPress any key to stop.".Dump(ConsoleColor.White);
Console.ReadKey();
return 0;

