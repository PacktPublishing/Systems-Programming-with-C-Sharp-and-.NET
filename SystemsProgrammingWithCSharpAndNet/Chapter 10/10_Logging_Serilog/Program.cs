// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using _10_Logging;
using ExtensionLibrary;
using Microsoft.Extensions.Logging;
using Serilog;

"Welcome to the wonderful world of logging!".Dump(ConsoleColor.Cyan);

// Setup the logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(@"c:\temp\log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341/")
    .CreateLogger();

// Set up DI
DependencyContainer.Setup();

var logger = DependencyContainer.Resolve<ILogger<Program>>();
if(logger == null)
    throw new NullReferenceException("Logger is null");

try
{

    logger.LogInformation("Hello, world!");
    logger.LogWarning("This is a warning");
    logger.LogError("This is an error");

    logger.LogInformation("This is a structured log message with {property1} and {property2}", 1, 2);
    throw new Exception("Something went wrong");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred");
}
finally
{
    Log.CloseAndFlush();
}

Console.WriteLine("Press any key to exit");
Console.ReadKey();