namespace RabbitMQLib
{
    public interface IBinding
    {
        string RoutingKey { get; }

        string QueueName { get; }

        string ExchangeName { get; }

        Dictionary<string, object>? Arguments { get; }
    }
}