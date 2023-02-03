using KinoSearch.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KinoSearch.Configuration.Configurations;
using StackExchange.Redis;

namespace KinoSearch.Configuration
{
    public class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json")
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddStackExchangeRedisCache(o =>
                {
                    o.Configuration = configuration.GetConnectionString("Redis");
                })
                .AddSingleton(typeof(IConfiguration), configuration)
                .AddConfigurationProcessors()
                .BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var configurationProcessors = scope.ServiceProvider.GetServices<IConfigurationProcessor>();

            Console.WriteLine($"--> Configuring...");
            foreach (var configurationProcessor in configurationProcessors)
            {
                try
                {
                    configurationProcessor.Process(scope, configuration);
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("Configuration processor of type [{0}] throws an exception: {1}",
                        configurationProcessor.GetType(), ex.Message);
                    Console.WriteLine();
                }
            }
            Console.WriteLine($"--> All done!");
        }
    }
}