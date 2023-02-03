namespace RabbitMQLib
{
    public interface IMessageBusClient<TConnection>
    {
        TConnection Connection { get; }
    }
}