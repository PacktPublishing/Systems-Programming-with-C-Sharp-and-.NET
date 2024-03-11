using ExtensionLibrary;
using ThreadSafety;


internal class Program
{
    private static volatile int _initialValue = 100;
    public static async Task<int> Main(string[] args)
    {
        Counter myCounter = new Counter(_initialValue);

        ThreadPool.QueueUserWorkItem(async (state) =>
        {
            await Task.Delay(500);
            //myCounter.InitialValue = 0;
            $"We are stopping it...".Dump(ConsoleColor.Red);
        });


        await WaitAWhile();

        $"In the main part of the app.".Dump(ConsoleColor.White);


        "Main app is done.\nPress any key to stop.".Dump(ConsoleColor.White);
        Console.ReadKey();
        return 0;

        async Task WaitAWhile()
        {
            var actualCounter = myCounter.InitialValue;
            do
            {
                $"In the loop at iterations {actualCounter}".Dump(ConsoleColor.Yellow);
                await Task.Delay(1);
            } while (--actualCounter > 0);
        }
    }
}

