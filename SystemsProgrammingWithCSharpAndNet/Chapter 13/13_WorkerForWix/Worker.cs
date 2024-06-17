namespace _13_WorkerForWix
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    string variableValue = Environment.GetEnvironmentVariable("VARIABLE_NAME");
                    
                    _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}, with variable {variableValue}" );
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
