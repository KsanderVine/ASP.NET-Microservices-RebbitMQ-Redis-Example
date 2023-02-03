using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQLib
{
    public class RabbitMQSubscriber : BackgroundService
    {
        protected abstract class Subscription
        {
            public readonly string QueueName;
            public abstract void Subscribe(IModel channel);

            public Subscription(string queueName)
            {
                QueueName = queueName;
            }
        }

        protected class Subscription<TMessageProcessor> : Subscription 
            where TMessageProcessor : IMessageProcessor
        {
            private readonly IServiceProvider _serviceProvider;

            private IModel? SubscriptionChannel { get; set; }

            public Subscription(string queueName, IServiceProvider serviceProvider) : base(queueName)
            {
                _serviceProvider = serviceProvider;
            }

            public override void Subscribe(IModel channel)
            {
                SubscriptionChannel = channel;
                EventingBasicConsumer channelConsumer = new(channel);
                channelConsumer.Received += Received;
                channel.BasicConsume(QueueName, true, channelConsumer);
            }

            private void Received(object? sender, BasicDeliverEventArgs e)
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                using IServiceScope scope = _serviceProvider.CreateScope();

                IMessageProcessor processor = scope.ServiceProvider.GetRequiredService<TMessageProcessor>();
                processor.Process(new RabbitMQMessage(message)
                {
                    ExchangeName = e.Exchange,
                    RoutingKey = e.RoutingKey
                });

                //SubscriptionChannel?.BasicAck(e.DeliveryTag, false);
            }
        }

        protected readonly IMessageBusClient<IConnection> _client;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IRabbitMQResolver _resolver;

        protected readonly List<Subscription> subscriptions = new();

        public RabbitMQSubscriber(
            IMessageBusClient<IConnection> client,
            IRabbitMQResolver resolver,
            IServiceProvider serviceProvider)
        {
            _client = client;
            _resolver = resolver;
            _serviceProvider = serviceProvider;
        }

        public RabbitMQSubscriber SubscribeQueue<TEventProcessor>(string queueName)
            where TEventProcessor : IMessageProcessor
        {
            subscriptions.Add(new Subscription<TEventProcessor>(queueName, _serviceProvider));
            return this;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach(var subscription in subscriptions)
            {
                _resolver.ResolveQueue(subscription.QueueName);

                var channel = _client.Connection.CreateModel();
                channel.BasicQos(0, 1, false);

                subscription.Subscribe(channel);
            }

            return Task.CompletedTask;
        }
    }
}