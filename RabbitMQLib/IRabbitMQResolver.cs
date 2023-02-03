namespace RabbitMQLib
{
    public interface IRabbitMQResolver
    {
        void ResolveExchange(string exchangeName);
        void ResolveQueue(string queueName);
    }
}