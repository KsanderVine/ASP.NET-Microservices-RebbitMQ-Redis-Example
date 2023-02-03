namespace RabbitMQLib
{
    public class RabbitMQMessage : IMessage
    {
        public string ExchangeName { get; set; } = String.Empty;
        public string RoutingKey { get; set; } = String.Empty;
        public object Body { get; set; }

        public RabbitMQMessage(object body)
        {
            Body = body;
        }
    }
}