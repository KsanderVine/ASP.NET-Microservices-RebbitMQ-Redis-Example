namespace RabbitMQLib
{
    public static class RabbitMQMessageExtensions
    {
        public static RabbitMQMessage ToExchange(this RabbitMQMessage rabbitMQMessage, string exchangeName)
        {
            rabbitMQMessage.ExchangeName = exchangeName;
            return rabbitMQMessage;
        }

        public static RabbitMQMessage WithRoutingKey(this RabbitMQMessage rabbitMQMessage, string routingKey)
        {
            rabbitMQMessage.RoutingKey = routingKey;
            return rabbitMQMessage;
        }
    }
}