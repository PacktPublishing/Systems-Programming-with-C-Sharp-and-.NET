using _14_CrossPlatformService;
using Mono.Unix;
using Mono.Unix.Native;

public class Program
{
    private static ILogger<Program> _log;

    public static void Main(string[] args)
    {
        
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();

        host.Run();
    }

}