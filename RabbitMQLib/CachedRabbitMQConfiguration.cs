using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RabbitMQLib
{
    public sealed class CachedRabbitMQConfiguration : RabbitMQConfiguration
    {
        private readonly IDistributedCache _cache;

        public CachedRabbitMQConfiguration(IDistributedCache cache)
        {
            _cache = cache;
            ReloadAll();
        }

        public override void ReloadAll()
        {
            var host = _cache.GetString("shared.rabbitmq.host");
            var uri = _cache.GetString("shared.rabbitmq.uri");

            var portString = _cache.GetString("shared.rabbitmq.port");
            var port = int.TryParse(portString, out int result) ? result : 5672;

            SetConnectionPort(port);
            SetConnectionHost(host ?? "localhost");
            SetConnectionUri(uri ?? "amqp://localhost:5672");

            var exchangesString = _cache.GetString("shared.rabbitmq.exchanges");
            var queuesString = _cache.GetString("shared.rabbitmq.queues");
            var bindingsString = _cache.GetString("shared.rabbitmq.bindings");

            IEnumerable<IExchange>? exchanges = JsonSerializer.Deserialize<IEnumerable<Exchange>>(exchangesString);
            IEnumerable<IQueue>? queues = JsonSerializer.Deserialize<IEnumerable<Queue>>(queuesString);
            IEnumerable<IBinding>? bindings = JsonSerializer.Deserialize<IEnumerable<Binding>>(bindingsString);

            if (exchanges != null)
            {
                foreach (var exchange in exchanges)
                {
                    string args = "";

                    if (exchange.Arguments != null)
                        args = string.Join(',', exchange.Arguments.Select(a => $"{a.Key} = {a.Value}"));

                    AddExchange(exchange);
                }
            }

            if (queues != null)
            {
                foreach (var queue in queues)
                {
                    string args = "";

                    if (queue.Arguments != null)
                        args = string.Join(',', queue.Arguments.Select(a => $"{a.Key} = {a.Value}"));

                    AddQueue(queue);
                }
            }

            if (bindings != null)
            {
                foreach (var binding in bindings)
                {
                    string args = "";

                    if (binding.Arguments != null)
                        args = string.Join(',', binding.Arguments.Select(a => $"{a.Key} = {a.Value}"));

                    AddBinding(binding);
                }
            }
        }
    }
}