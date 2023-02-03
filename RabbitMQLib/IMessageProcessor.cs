namespace RabbitMQLib
{
    public interface IMessageProcessor
    {
        void Process(IMessage message);
    }
}