using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace RabbitMQLib
{
    public class DevelopmentRabbitMQConfiguration : RabbitMQConfiguration
    {
        public DevelopmentRabbitMQConfiguration()
        {
            ReloadAll();
        }

        public override void ReloadAll()
        {
            AddExchange(new Exchange("Data_Topic", "topic", true, false));
            AddQueue(new Queue("Users", true, false, false));
            AddQueue(new Queue("Films", true, false, false));
            AddBinding(new Binding("user.#", "Users", "Data_Topic"));
            AddBinding(new Binding("film.#", "Films", "Data_Topic"));
        }
    }
}