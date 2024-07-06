using System.Diagnostics;
using ExtensionLibrary;

#pragma warning disable CA1416

var counter = 0;
var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
while (true)
{
    if (counter++ == 10)
        // Start a method on a background thread
        Task.Run(() =>
        {
            Parallel.For(0, Environment.ProcessorCount, j =>
            {
                for (var i = 0; i < 100000000; i++)
                {
                    var result = Math.Exp(Math.Sqrt(Math.PI));
                }
            });

            counter = 0;
        });

    var cpuUsage = cpuCounter.NextValue();
    var message = $"CPU Usage: {cpuUsage}%";
    var color = cpuUsage > 10 ? ConsoleColor.Red : ConsoleColor.Green;

    message.Dump(color);
    await Task.Delay(200);
}