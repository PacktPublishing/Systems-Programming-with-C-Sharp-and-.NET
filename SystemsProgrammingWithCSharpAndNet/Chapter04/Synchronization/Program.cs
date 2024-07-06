using ExtensionLibrary;

"In the main part of the app.".Dump(ConsoleColor.White);
using var cts = new CancellationTokenSource();

var task1 = DoSomethingForOneSecondAsync(cts.Token);
await Task.Delay(500);
"We got bored. Let's cancel.".Dump(ConsoleColor.White);
cts.Cancel();
Task.WaitAny(task1);

"Main app is done.\nPress any key to stop.".Dump(ConsoleColor.White);
Console.ReadKey();
return 0;

async Task DoSomethingForOneSecondAsync(CancellationToken cancellationToken)
{
    $"Doing something for one second.".Dump(ConsoleColor.Yellow);
    bool hasBeenCancelled = false;
    int i = 0;
    for (i = 0; i < 1000; i++)
    {
        if (cancellationToken.IsCancellationRequested)
        {   
            hasBeenCancelled = true;
            break;
        }

        await Task.Delay(1, cancellationToken);
    }

    if(hasBeenCancelled)
    {
        $"We got interrupted after {i} iterations.".Dump(ConsoleColor.Yellow);
    }
    else
    {
        $"Finished something for one second".Dump(ConsoleColor.Yellow);
    }
}

async Task<int> DoSomethingForTwoSecondsAsync()
{
    "Doing something for two seconds.".Dump(ConsoleColor.DarkYellow);
    await Task.Delay(2000);
    "Done doing something for two seconds.".Dump(ConsoleColor.DarkYellow);
    return 2;
}