using ExtensionLibrary;

namespace _11_MultiThreaded
{
    internal class VariableCheck
    {
        public void Run()
        {
            var rnd = new Random();

            for (int i = 0; i < 2; i++)
            {
                int threadNumber = i;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    var counter = 0;
                    while (true)
                    {
                        $"Thread {threadNumber} with counter {counter++}".Dump(ConsoleColor.Yellow);
                        Task.Delay(rnd.Next(1000)).Wait();
                    }
                });
            }
        }
    }
}
