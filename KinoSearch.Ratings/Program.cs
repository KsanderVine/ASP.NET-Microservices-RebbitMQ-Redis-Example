using KinoSearch.Ratings.Data;
using KinoSearch.Ratings.Services;
using RabbitMQLib;

namespace KinoSearch.Ratings
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddStackExchangeRedisCache(options =>
                options.Configuration = builder.Configuration.GetConnectionString("Redis"));

            builder.Services.AddSingleton<IFilmsRepository, FilmsRepository>();
            builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
            builder.Services.AddSingleton<IRatingsRepository, RatingsRepository>();

            builder.Services.AddScoped<FilmsQueueMessageProcessor>();
            builder.Services.AddScoped<UsersQueueMessageProcessor>();

            builder.Services.AddRabbitMQ<CachedRabbitMQConfiguration>();
            builder.Services.AddRabbitMQSubscriber(s => s
                .SubscribeQueue<FilmsQueueMessageProcessor>("Films_Ratings")
                .SubscribeQueue<UsersQueueMessageProcessor>("Users_Ratings"));

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