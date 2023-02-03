namespace RabbitMQLib
{
    public class Binding : IBinding
    {
        public string RoutingKey { get; set;  }

        public string QueueName { get; set; }

        public string ExchangeName { get; set; }

        public Dictionary<string, object>? Arguments { get; set; }

        public Binding()
        {
            RoutingKey = "";
            QueueName = "";
            ExchangeName = "";
            Arguments = null;
        }

        public Binding(
            string routingKey, 
            string queueName, 
            string exchangeName,
            Dictionary<string, object>? arguments = null)
        {
            RoutingKey = routingKey;
            QueueName = queueName;
            ExchangeName = exchangeName;
            Arguments = arguments;
        }
    }
}