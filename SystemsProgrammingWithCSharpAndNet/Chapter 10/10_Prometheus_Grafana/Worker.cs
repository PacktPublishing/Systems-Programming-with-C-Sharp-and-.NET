using System.Diagnostics;
using Prometheus;

namespace _10_Prometheus_Grafana
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Gauge _memoryGauge;
        private int _counter;
        private byte[]? _dummyObject;


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _memoryGauge = Metrics.CreateGauge("dotnet_application_memory_bytes", "Shows the current memory usage");

            _counter = 0;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    _memoryGauge.Set(Process.GetCurrentProcess().WorkingSet64);

                    // Simulate a memory leak
                    if (_counter++ == 10)
                    {
                        
                            // Create a dummy large object
                            _dummyObject = new byte[100000000];
                    }

                    if (_counter == 20)
                    {
                        _counter = 0;
                        _dummyObject = null;
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
