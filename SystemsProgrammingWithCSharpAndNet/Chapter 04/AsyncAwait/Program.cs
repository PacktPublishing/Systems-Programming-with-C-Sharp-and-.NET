using ExtensionLibrary;

"Just before calling the long-running DoWork()"
    .Dump(ConsoleColor.Blue);
var result = await DoWork();

// The program is paused until DoWork is finished. 
// This is a waste of CPU!

$"Program has finished. Final result = {result}".Dump(ConsoleColor.Blue);
Console.ReadKey();
return;


int Wrapper()
{
    return DoWork().Result;
}

async Task<int> DoWork()
{
    "We are doing important stuff!".Dump(ConsoleColor.DarkYellow);

    int a = 0;
    // Do something useful, then wait a bit.
    await Task.Run(async () =>
    {
        "Inside the Task, before the loop".Dump(ConsoleColor.Yellow);
        for (int i = 0; i < 1000; i++)
        {
            a = i + 1;
            await Task.Delay(1);
            if(i>500)
                throw new Exception("Something went wrong!");

        }
        "Inside the Task, after the loop".Dump(ConsoleColor.Yellow);

    });
    "After running the long loop.".Dump(ConsoleColor.DarkYellow);

    return a;
}