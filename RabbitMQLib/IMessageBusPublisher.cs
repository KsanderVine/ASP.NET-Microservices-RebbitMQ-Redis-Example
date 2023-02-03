namespace RabbitMQLib
{
    public interface IMessageBusPublisher
    {
        void Publish(IMessage message);
    }
}