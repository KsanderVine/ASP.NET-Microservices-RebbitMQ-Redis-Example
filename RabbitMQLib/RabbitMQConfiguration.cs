namespace RabbitMQLib
{
    public abstract class RabbitMQConfiguration
    {
        private readonly ConnectionSettings connectionSettings = new();
        private readonly List<IExchange> exchanges = new();
        private readonly List<IQueue> queues = new();
        private readonly List<IBinding> bindings = new();


        protected void SetConnectionUri(string uri) => connectionSettings.Uri = uri;

        protected void SetConnectionHost(string host) => connectionSettings.Host = host;

        protected void SetConnectionPort(int port) => connectionSettings.Port = port;


        protected void AddExchange(IExchange exchange) => exchanges.Add(exchange);

        protected void AddQueue(IQueue queue) => queues.Add(queue);

        protected void AddBinding(IBinding binding) => bindings.Add(binding);


        public IEnumerable<IExchange> GetAllExchanges() => exchanges.ToList();

        public IEnumerable<IQueue> GetAllQueues() => queues.ToList();

        public IEnumerable<IBinding> GetAllBindings() => bindings.ToList();


        public IEnumerable<IBinding> GetBindingsByExchangeName(string exchangeName)
        {
            return bindings.Where(b => string.Equals(b.ExchangeName, exchangeName));
        }

        public IEnumerable<IBinding> GetBindingsByQueueName(string queueName)
        {
            return bindings.Where(b => string.Equals(b.QueueName, queueName));
        }


        public IExchange? GetExchange(string exchangeName)
        {
            return exchanges.FirstOrDefault(e => string.Equals(e.ExchangeName, exchangeName));
        }

        public IQueue? GetQueue(string queueName)
        {
            return queues.FirstOrDefault(q => string.Equals(q.QueueName, queueName));
        }


        public ConnectionSettings GetConnectionSettings() => connectionSettings;

        public abstract void ReloadAll();
    }
}