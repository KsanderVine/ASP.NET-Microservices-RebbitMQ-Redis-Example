using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQLib
{
    public class RabbitMQPublisher : IMessageBusPublisher
    {
        private readonly IMessageBusClient<IConnection> _client;
        private readonly IRabbitMQResolver _resolver;

        public RabbitMQPublisher(IMessageBusClient<IConnection> client, IRabbitMQResolver resolver)
        {
            _client = client;
            _resolver = resolver;
        }

        public void Publish(IMessage message)
        {
            var (exchange, routingKey, body) = DeconstructMessage(message);

            _resolver.ResolveExchange(exchange);

            using (var channel = _client.Connection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                channel.BasicPublish(exchange, routingKey, false, properties, body);
            }

            static (string, string, ReadOnlyMemory<byte>) DeconstructMessage(IMessage message)
            {
                if (message is RabbitMQMessage rabbitMQMessage)
                {
                    return (rabbitMQMessage.ExchangeName, rabbitMQMessage.RoutingKey, GetBodyEncoded(message.Body));
                }
                else
                {
                    return ("", "", GetBodyEncoded(message.Body));
                }
            }

            static ReadOnlyMemory<byte> GetBodyEncoded(object body)
            {
                var serializedBody = JsonSerializer.Serialize(body);
                return Encoding.UTF8.GetBytes(serializedBody);
            }
        }
    }
}