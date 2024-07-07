using _13_WorkerForSetup;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();

    }).UseWindowsService();


await builder.RunConsoleAsync();

