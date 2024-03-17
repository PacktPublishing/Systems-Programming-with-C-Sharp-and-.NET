#define FAKESERIAL

using _09_SerialMonitor;
using _09_SerialMonitor.Fakes;

var builder = Host.CreateApplicationBuilder(args);

#if FAKESERIAL
    builder.Services.AddTransient<IComPortWatcher, FakeComPortWatcher>();
    builder.Services.AddTransient<IAsyncSerial, FakeAsyncSerial>();
#else
    builder.Services.AddTransient<IComPortWatcher, ComPortWatcher>();
    builder.Services.AddTransient<IAsyncSerial, AsyncSerial>();
#endif

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();