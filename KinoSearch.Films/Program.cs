using KinoSearch.Films.Data;
using RabbitMQLib;

namespace KinoSearch.Films
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddStackExchangeRedisCache(options =>
                options.Configuration = builder.Configuration.GetConnectionString("Redis"));

            builder.Services.AddSingleton<IFilmsRepository, FilmsRepository>();

            builder.Services.AddRabbitMQ<CachedRabbitMQConfiguration>();
            builder.Services.AddRabbitMQPublisher();

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