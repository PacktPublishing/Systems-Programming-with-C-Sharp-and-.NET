using Serilog;
using Serilog.Enrichers;
using Serilog.Formatting.Json;

var logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.File(
        new JsonFormatter(),
        "logs\\log.json",
        rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();


try
{
    logger.Verbose("This is verbose");
    logger.Debug("This is debug");
    logger.Information("This is information");
    logger.Warning("This is warning");
    logger.Error("This is error");
    logger.Fatal("This is fatal");

    logger.Debug(
        "The user with userId {userId} logged in at {loggedInTime}",
        42,
        DateTime.UtcNow.TimeOfDay);

}
finally
{
    await Log.CloseAndFlushAsync();
}

Console.ReadKey(true);