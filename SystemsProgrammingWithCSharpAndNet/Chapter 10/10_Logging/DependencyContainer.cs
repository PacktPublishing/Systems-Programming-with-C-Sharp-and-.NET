using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace _10_Logging
{
    internal static class DependencyContainer
    {
        
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static ServiceProvider _serviceProvider;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public static void Setup()
        {
            var services = new ServiceCollection();
            RegisterServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void RegisterServices(ServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
            });
        }


        public static TService? Resolve<TService>()
        {
            return _serviceProvider.GetService<TService>();
        }
    }
}
