using NLog;
using NLog.Config;

LogManager.Configuration =
    new XmlLoggingConfiguration(
        "NLog.config"
    );

try
{
    var logger = LogManager.GetCurrentClassLogger();
    var otherLogger  = LogManager.GetLogger("OtherLogger");

    while (true)
    {
        logger.Trace("This is a trace message");
        logger.Debug("This is a debug message");
        logger.Info("Application started");
        logger.Warn("This is a warning message");
        logger.Error("This is an error message");
        logger.Fatal("This is a fatal error message");

        otherLogger.Info("This is a message from the other logger");
        await Task.Delay(1000);
    }
}
finally
{
    LogManager.Shutdown();
}