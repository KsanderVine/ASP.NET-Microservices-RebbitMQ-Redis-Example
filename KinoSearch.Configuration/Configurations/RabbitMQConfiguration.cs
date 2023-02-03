using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQLib;
using System.Text.Json;

namespace KinoSearch.Configuration.Configurations
{
    public class RabbitMQConfiguration : IConfigurationProcessor
    {
        private class RabbitMQOptions
        {
            public const string Section = nameof(RabbitMQOptions);

            public string? Uri { get; set; }
            public string? Host { get; set; }
            public int? Port { get; set; }

            public Exchange[]? Exchanges { get; set; }
            public Queue[]? Queues { get; set; }
            public Binding[]? Bindings { get; set; }
        }

        public void Process(IServiceScope scope, IConfiguration configuration)
        {
            var cacheService = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            Console.WriteLine($"--> Configuration processor {typeof(RabbitMQConfiguration).Name} started...");

            var options = configuration
                .GetSection(RabbitMQOptions.Section)
                .Get<RabbitMQOptions>();

            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.MaxValue
            };

            if (!string.IsNullOrWhiteSpace(options.Uri))
            {
                cacheService.SetString("shared.rabbitmq.uri", options.Uri, cacheOptions);
            }

            if (!string.IsNullOrWhiteSpace(options.Host))
            {
                cacheService.SetString("shared.rabbitmq.host", options.Host, cacheOptions);
            }

            if (options.Port != null)
            {
                cacheService.SetString("shared.rabbitmq.port", options.Port.ToString(), cacheOptions);
            }

            if (options.Exchanges != null && options.Exchanges.Any())
            {
                var serializedValue = JsonSerializer.Serialize(options.Exchanges);
                Console.WriteLine(serializedValue);
                cacheService.SetString("shared.rabbitmq.exchanges", serializedValue, cacheOptions);
            }

            if (options.Queues != null && options.Queues.Any())
            {
                var serializedValue = JsonSerializer.Serialize(options.Queues);
                Console.WriteLine(serializedValue);
                cacheService.SetString("shared.rabbitmq.queues", serializedValue, cacheOptions);
            }

            if (options.Bindings != null && options.Bindings.Any())
            {
                var serializedValue = JsonSerializer.Serialize(options.Bindings);
                Console.WriteLine(serializedValue);
                cacheService.SetString("shared.rabbitmq.bindings", serializedValue, cacheOptions);
            }

            Console.WriteLine($"--> Configuration processor {typeof(RabbitMQConfiguration).Name} finished...");
        }
    }
}