// See https://aka.ms/new-console-template for more information

using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Formatting.Json;

//Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine(msg));

var logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console(new JsonFormatter())
    //.WriteTo.File(
    //    new JsonFormatter(),
    //    "logs\\log.json",
    //    rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://alienware-dv:8080", LogEventLevel.Verbose)
    .Enrich.With(new ThreadIdEnricher())
    .CreateLogger();

Serilog.Debugging.SelfLog.Enable(Console.Out);

try
{
    logger.Verbose("This is verbose");
    logger.Debug("This is debug");
    logger.Information("This is information");
    logger.Warning("This is warning");
    logger.Error("This is error");
    logger.Fatal("This is fatal");
    logger.Information("Some information");
    logger.Information(
        "The user with userId {userId} logged in at {loggedInTime}", 
        42, 
        DateTime.UtcNow.TimeOfDay);
}
finally
{
    await Log.CloseAndFlushAsync();
}