// See https://aka.ms/new-console-template for more information

using Serilog;
using Serilog.Formatting.Json;

var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(
        new JsonFormatter(),
        "logs\\log.json",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    logger.Verbose("This is verbose");
    logger.Debug("This is debug");
    logger.Information("This is information");
    logger.Warning("This is warning");
    logger.Error("This is error");
    logger.Fatal("This is fatal");
}
finally
{
    await Log.CloseAndFlushAsync();
}