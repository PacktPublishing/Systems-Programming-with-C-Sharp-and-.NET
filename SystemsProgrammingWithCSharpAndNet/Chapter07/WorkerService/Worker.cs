namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<Worker> _logger;
    private int _counter;

    public Worker(
        ILogger<Worker> logger,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
            if (_counter++ >= 9) break;
        }

        _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
        _hostApplicationLifetime.StopApplication();
    }
}