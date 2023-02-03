namespace RabbitMQLib
{
    public interface IExchange
    {
        string ExchangeName { get; }

        string ExchangeType { get; }

        bool IsDurable { get; }

        bool IsAutoDelete { get; }

        Dictionary<string, object>? Arguments { get; }
    }
}