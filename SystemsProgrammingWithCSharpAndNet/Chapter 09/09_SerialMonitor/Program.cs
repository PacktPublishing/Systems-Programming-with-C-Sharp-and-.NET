using _09_SerialMonitor;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<IComPortWatcher, ComPortWatcher>();
builder.Services.AddTransient<IAsyncSerial, AsyncSerial>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();