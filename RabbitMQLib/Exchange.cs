namespace RabbitMQLib
{
    public class Exchange : IExchange
    {
        public string ExchangeName { get; set; }

        public string ExchangeType { get; set; }

        public bool IsDurable { get; set; }

        public bool IsAutoDelete { get; set; }

        public Dictionary<string, object>? Arguments { get; set; }

        public Exchange()
        {
            ExchangeName = "";
            ExchangeType = "";
            IsDurable = true;
            IsAutoDelete = false;
            Arguments = null;
        }

        public Exchange(
            string exchangeName, 
            string exchangeType, 
            bool isDurable, 
            bool isAutoDelete,
            Dictionary<string, object>? arguments = null)
        {
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
            IsDurable = isDurable;
            IsAutoDelete = isAutoDelete;
            Arguments = arguments;
        }
    }
}