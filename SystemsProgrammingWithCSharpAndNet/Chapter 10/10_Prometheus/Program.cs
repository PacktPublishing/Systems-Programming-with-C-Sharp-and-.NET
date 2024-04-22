using ExtensionLibrary;
using Prometheus;

#pragma warning disable CA1416


Gauge memoryGauge = 
    Metrics.CreateGauge(
        "app_memory_usage_bytes",
        "Memory Usage of the application in bytes.");

var server = 
    new MetricServer(
        hostname:"127.0.0.1", 
        port: 1234);
server.Start();

var counter = 0;
List<byte[]> buffer = [];
Random rnd = new Random();

while (true)
{
    if (counter++ % 5 == 0)
        AllocateMemoryBlock(rnd, buffer);

    if (counter == 20)
    {
        ClearMemory(buffer);
        counter = 0;
    }

    UpdateMemoryGauge(memoryGauge);
    await Task.Delay(1000);
}

return;

static void UpdateMemoryGauge(Gauge memoryGauge)
{
    var memoryUsage = GC.GetTotalMemory(forceFullCollection: false);
    memoryGauge.Set(memoryUsage);
}

static void AllocateMemoryBlock(Random random, List<byte[]> bytesList)
{
    var memoryToAllocate = 
        random.Next(50000000, 200000000);
    var dummyBlock = 
        new byte[memoryToAllocate];
    bytesList.Add(dummyBlock);
    "Memory block added".Dump(ConsoleColor.Blue);
}

static void ClearMemory(List<byte[]> list)
{
    list.Clear();
    GC.Collect();
    "Memory block cleared".Dump(ConsoleColor.Green);
}