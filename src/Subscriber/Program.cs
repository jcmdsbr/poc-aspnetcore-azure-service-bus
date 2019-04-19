using System;
using System.Threading.Tasks;
using Core.Configurations;
using Core.Contracts;
using Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static System.String;
namespace Subscriber
{
    internal class Program
    {
        private static async Task  Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(AddConfiguration)
                .ConfigureServices(RegisterServices)
                .ConfigureLogging(AddLogging)
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }

        private static void RegisterServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var serviceBusConfigurations = new ServiceBusConfiguration();

            new ConfigureFromConfigurationOptions<ServiceBusConfiguration>(
                hostContext.Configuration.GetSection("ServiceBusConfigurations"))
                                .Configure(serviceBusConfigurations);

            services.AddSingleton(serviceBusConfigurations);
            services.AddScoped<IProductHandlerService, ProductService>();
            services.AddHostedService<QueueListener>();
        }

        private static void AddConfiguration(HostBuilderContext hostContext, IConfigurationBuilder configApp)
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            
            if (!IsNullOrEmpty(environment))
            {
                hostContext.HostingEnvironment.EnvironmentName = environment;
            }

            configApp.AddJsonFile("appsettings.json");
            configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
            configApp.AddEnvironmentVariables();
        }

        private static void AddLogging(HostBuilderContext hostContext, ILoggingBuilder configLogging)
        {
            configLogging.AddConsole();
            configLogging.AddDebug();
        }

    }
}
