using Microsoft.Extensions.Configuration;

namespace RabbitMQLib
{
    public sealed class AppSettingsRabbitMQConfiguration : RabbitMQConfiguration
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

        private readonly IConfiguration _configuration;

        public AppSettingsRabbitMQConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            ReloadAll();
            
        }

        public override void ReloadAll()
        {
            var options = _configuration
                .GetSection(RabbitMQOptions.Section)
                .Get<RabbitMQOptions>();

            SetConnectionPort(options.Port ?? 5672);
            SetConnectionHost(options.Host ?? "localhost");
            SetConnectionUri(options.Uri ?? "amqp://localhost:5672");

            if (options.Exchanges != null)
            {
                foreach (var exchange in options.Exchanges)
                {
                    AddExchange(exchange);
                }
            }

            if (options.Queues != null)
            {
                foreach (var queue in options.Queues)
                {
                    AddQueue(queue);
                }
            }

            if (options.Bindings != null)
            {
                foreach (var binding in options.Bindings)
                {
                    AddBinding(binding);
                }
            }
        }
    }
}