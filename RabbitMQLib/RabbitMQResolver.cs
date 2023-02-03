using RabbitMQ.Client;

namespace RabbitMQLib
{
    public class RabbitMQResolver : IRabbitMQResolver
    {
        private readonly IMessageBusClient<IConnection> _client;
        private readonly RabbitMQConfiguration _configuration;

        public RabbitMQResolver(IMessageBusClient<IConnection> client, RabbitMQConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public void ResolveExchange(string exchangeName)
        {
            using var channel = _client.Connection.CreateModel();

            if (_configuration.GetExchange(exchangeName) is IExchange exchange)
            {
                channel.ExchangeDeclare(
                    exchange.ExchangeName,
                    exchange.ExchangeType,
                    exchange.IsDurable,
                    exchange.IsAutoDelete,
                    exchange.Arguments);
            }
            else
            {
                throw new NullReferenceException($"Can not find any exchange configuration with name {exchangeName}");
            }
        }

        public void ResolveQueue(string queueName)
        {
            using var channel = _client.Connection.CreateModel();

            if (_configuration.GetQueue(queueName) is IQueue queue)
            {
                channel.QueueDeclare(
                    queue.QueueName,
                    queue.IsDurable,
                    queue.IsExclusive,
                    queue.IsAutoDelete,
                    queue.Arguments);

                var bindings = _configuration.GetBindingsByQueueName(queueName);
                var exchangeNames = bindings.Select(b => b.ExchangeName).ToHashSet();
                var exchanges = _configuration.GetAllExchanges().Where(x => exchangeNames.Contains(x.ExchangeName));

                channel.QueueDeclare();

                foreach (var exchange in exchanges)
                {
                    channel.ExchangeDeclare(
                        exchange.ExchangeName,
                        exchange.ExchangeType,
                        exchange.IsDurable,
                        exchange.IsAutoDelete,
                        exchange.Arguments);
                }

                foreach (var binding in bindings)
                {
                    channel.QueueBind(
                        binding.QueueName,
                        binding.ExchangeName,
                        binding.RoutingKey,
                        binding.Arguments);
                }
            }
            else
            {
                throw new NullReferenceException($"Can not find any queue configuration with name {queueName}");
            }
        }
    }
}