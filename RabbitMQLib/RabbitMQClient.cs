using RabbitMQ.Client;

namespace RabbitMQLib
{
    public class RabbitMQClient : IMessageBusClient<IConnection>
    {
        public IConnection Connection { get; private set; }

        public RabbitMQClient(RabbitMQConfiguration configuration)
        {
            string? rabbitMQUri = configuration.GetConnectionSettings().Uri;

            ConnectionFactory connectionFactory = new()
            {
                Uri = new Uri(rabbitMQUri ?? "amqp://localhost:5672"),
                AutomaticRecoveryEnabled = true,
                DispatchConsumersAsync = false
            };

            var connectionName = AppDomain.CurrentDomain.FriendlyName;
            Connection = connectionFactory.CreateConnection(connectionName);
        }
    }
}