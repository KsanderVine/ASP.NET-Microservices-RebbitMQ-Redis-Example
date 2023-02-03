namespace RabbitMQLib
{
    public interface IQueue
    {
        string QueueName { get; }

        bool IsDurable { get; }

        bool IsExclusive { get; }

        bool IsAutoDelete { get; }

        Dictionary<string, object>? Arguments { get; }
    }
}