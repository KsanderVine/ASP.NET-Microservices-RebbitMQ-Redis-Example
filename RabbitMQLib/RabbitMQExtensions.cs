using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace RabbitMQLib
{
    public static class RabbitMQExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection collection)
        {
            collection.AddSingleton<RabbitMQConfiguration, AppSettingsRabbitMQConfiguration>();
            collection.AddSingleton<IMessageBusClient<IConnection>, RabbitMQClient>();
            collection.AddSingleton<IRabbitMQResolver, RabbitMQResolver>();
            return collection;
        }

        public static IServiceCollection AddRabbitMQ<TConfiguration> (this IServiceCollection collection) 
            where TConfiguration : RabbitMQConfiguration
        {
            collection.AddSingleton<RabbitMQConfiguration, TConfiguration>();
            collection.AddSingleton<IMessageBusClient<IConnection>, RabbitMQClient>();
            collection.AddSingleton<IRabbitMQResolver, RabbitMQResolver>();
            return collection;
        }

        public static IServiceCollection AddRabbitMQPublisher(this IServiceCollection collection)
        {
            collection.AddSingleton<IMessageBusPublisher, RabbitMQPublisher>();
            return collection;
        }

        public static IServiceCollection AddRabbitMQPublisher<TPublisher>(this IServiceCollection collection)
            where TPublisher : class, IMessageBusPublisher
        {
            collection.AddSingleton<IMessageBusPublisher, TPublisher>();
            return collection;
        }

        public static IServiceCollection AddRabbitMQSubscriber(
            this IServiceCollection services,
            Func<RabbitMQSubscriber, RabbitMQSubscriber> factory)
        {
            services.AddHostedService<RabbitMQSubscriber>(x =>
            {
                var client = x.GetRequiredService<IMessageBusClient<IConnection>>();
                var resolver = x.GetRequiredService<IRabbitMQResolver>();
                var serviceProvider = x.GetRequiredService<IServiceProvider>();

                return factory(new RabbitMQSubscriber(client, resolver, serviceProvider));
            });
            return services;
        }
    }
}