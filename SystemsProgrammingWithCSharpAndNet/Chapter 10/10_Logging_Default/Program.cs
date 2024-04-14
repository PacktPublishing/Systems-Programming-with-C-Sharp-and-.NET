using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true);

var configuration = builder.Build();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConfiguration(configuration.GetSection("Logging"));
    builder.AddConsole();
});

var logger = loggerFactory.CreateLogger<Program>();
while (true)
{
    logger.LogInformation("This is information");
    logger.LogWarning("This is a warning");
    logger.LogTrace("This is a trace message");
    logger.LogError("This is an error");
    logger.LogCritical("This is a critical message");
    await Task.Delay(1000);
}

