using KinoSearch.Users.Data;
using RabbitMQ.Client;
using RabbitMQLib;
using StackExchange;

namespace KinoSearch.Users
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddStackExchangeRedisCache(options =>
                options.Configuration = builder.Configuration.GetConnectionString("Redis"));

            builder.Services.AddSingleton<IUsersRepository, UsersRepository>();

            builder.Services.AddRabbitMQ<CachedRabbitMQConfiguration>();
            builder.Services.AddRabbitMQPublisher<RabbitMQPublisher>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}