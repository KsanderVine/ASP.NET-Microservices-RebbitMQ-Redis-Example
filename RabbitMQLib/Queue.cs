namespace RabbitMQLib
{
    public class Queue : IQueue
    {
        public string QueueName { get; set; }

        public bool IsDurable { get; set; }

        public bool IsExclusive { get; set; }

        public bool IsAutoDelete { get; set; }

        public Dictionary<string, object>? Arguments { get; set; }

        public Queue()
        {
            QueueName = "";
            IsDurable = true;
            IsExclusive = false;
            IsAutoDelete = false;
            Arguments = null;
        }

        public Queue(
            string queueName, 
            bool isDurable, 
            bool isExclusive, 
            bool isAutoDelete,
            Dictionary<string, object>? arguments = null)
        {
            QueueName = queueName;
            IsDurable = isDurable;
            IsExclusive = isExclusive;
            IsAutoDelete = isAutoDelete;
            Arguments = arguments;
        }
    }
}