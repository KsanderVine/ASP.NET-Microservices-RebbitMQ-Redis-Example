using KinoSearch.Ratings.Data;
using KinoSearch.Ratings.Models;
using RabbitMQLib;
using System.Text.Json;

namespace KinoSearch.Ratings.Services
{
    public class UsersQueueMessageProcessor : IMessageProcessor
    {
        private readonly ILogger<UsersQueueMessageProcessor> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IRatingsRepository _ratingsRepository;

        public UsersQueueMessageProcessor(
            ILogger<UsersQueueMessageProcessor> logger,
            IUsersRepository usersRepository,
            IRatingsRepository ratingsRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _ratingsRepository = ratingsRepository;
        }

        public void Process(IMessage originalMessage)
        {
            _logger.LogInformation("Taking message...");

            if (originalMessage is RabbitMQMessage message)
            {
                switch (message.RoutingKey)
                {
                    case "user.created":
                        UserCreated(DeserializeBody<User>(message.Body));
                        break;
                    case "user.deleted":
                        UserDeleted(DeserializeBody<User>(message.Body));
                        break;
                    default:
                        _logger.LogError("Unrecognized RoutingKey for this message processor {RoutingKey}", message.RoutingKey);
                        break;
                }

                _logger.LogInformation("Message processed!");
            }
            else
            {
                _logger.LogError("Can not recognize message type");
            }
        }

        private static TModel DeserializeBody<TModel>(object body)
        {
            if (JsonSerializer.Deserialize<TModel>((string)body) is TModel model)
            {
                return model;
            }
            else
            {
                throw new Exception($"Error while deserializing type {typeof(TModel).Name}");
            }
        }

        private void UserCreated(User createdUser)
        {
            _logger.LogInformation("Message processed by \"{Processor}\" method", nameof(UserCreated));

            createdUser.ExternalId = createdUser.Id;
            _usersRepository.Create(createdUser);

            _logger.LogInformation("Result: {Result}", JsonSerializer.Serialize(createdUser));
        }

        private void UserDeleted(User deletedUser)
        {
            _logger.LogInformation("Message processed by \"{Processor}\" method", nameof(UserDeleted));

            if (_usersRepository.GetByExternalId(deletedUser.Id) is User user)
            {
                _logger.LogInformation("Deleting film: UserId: {User}; Deleted: {IsDeleted}",
                    user.ExternalId, _usersRepository.Delete(user));

                var ratings = _ratingsRepository.GetByUserId(user.ExternalId);
                foreach (var rating in ratings)
                {
                    _logger.LogInformation("Deleting rating: FilmId:{FilmId}; UserId:{UserId}; Deleted: {IsDeleted}",
                        rating.FilmId, rating.UserId, _ratingsRepository.Delete(rating));
                }
            }
        }
    }
}