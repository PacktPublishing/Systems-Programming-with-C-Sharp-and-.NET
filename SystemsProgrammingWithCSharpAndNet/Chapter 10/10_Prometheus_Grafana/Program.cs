using _10_Prometheus_Grafana;
using Prometheus;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var server = new MetricServer(port:1234, hostname:"127.0.0.1");
server.Start();


var host = builder.Build();
host.Run();
