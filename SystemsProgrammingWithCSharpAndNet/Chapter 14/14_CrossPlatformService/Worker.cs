using ExtensionLibrary;
using Mono.Unix;
using Mono.Unix.Native;

namespace _14_CrossPlatformService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        RegisterSignalHandlers();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void SignalHandler(Signum signal)
    {
        switch (signal)
        {
            case Signum.SIGINT:
                _logger.LogInformation("Received SIGINT");
                break;
            case Signum.SIGTERM:
                _logger.LogInformation("Received SIGTERM");
                break;
            default:
                _logger.LogInformation($"Received signal {(int)signal}");
                break;
        }


        Environment.Exit(0);
    }

    private void RegisterSignalHandlers()
    {
        // This is the default behavior for SIGTERM
        AppDomain.CurrentDomain.ProcessExit += 
            (sender, eventArgs) => $"Process exit".Dump();

        // Handle the signals
        UnixSignal[] signals =
        {
            new(Signum.SIGINT),
            new(Signum.SIGTERM)
        };

        var signalThread = new Thread(() =>
        {
            while (true)
            {
                var index = UnixSignal.WaitAny(signals);
                SignalHandler(signals[index].Signum);
            }
        })
        {
            IsBackground = true
        };

        signalThread.Start();
    }
}