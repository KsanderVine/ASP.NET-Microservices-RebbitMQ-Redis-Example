using KinoSearch.Comments.Data;
using KinoSearch.Comments.Services;
using RabbitMQLib;

namespace KinoSearch.Comments
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
            builder.Services.AddSingleton<ICommentsRepository, CommentsRepository>();

            builder.Services.AddScoped<FilmsQueueMessageProcessor>();
            builder.Services.AddScoped<UsersQueueMessageProcessor>();

            builder.Services.AddRabbitMQ<CachedRabbitMQConfiguration>();
            builder.Services.AddRabbitMQSubscriber(s => s
                .SubscribeQueue<FilmsQueueMessageProcessor>("Films_Comments")
                .SubscribeQueue<UsersQueueMessageProcessor>("Users_Comments"));

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