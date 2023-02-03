using KinoSearch.Ratings.Data;
using KinoSearch.Ratings.Models;
using RabbitMQLib;
using System.Text.Json;

namespace KinoSearch.Ratings.Services
{
    public class FilmsQueueMessageProcessor : IMessageProcessor
    {
        private readonly ILogger<FilmsQueueMessageProcessor> _logger;
        private readonly IFilmsRepository _filmsRepository;
        private readonly IRatingsRepository _ratingsRepository;

        public FilmsQueueMessageProcessor(
            ILogger<FilmsQueueMessageProcessor> logger, 
            IFilmsRepository filmsRepository,
            IRatingsRepository ratingsRepository)
        {
            _logger = logger;
            _filmsRepository = filmsRepository;
            _ratingsRepository = ratingsRepository;
        }

        public void Process(IMessage originalMessage)
        {
            _logger.LogInformation("Taking message...");

            if (originalMessage is RabbitMQMessage message)
            {
                switch (message.RoutingKey)
                {
                    case "film.created":
                        FilmCreated(DeserializeBody<Film>(message.Body));
                        break;
                    case "film.deleted":
                        FilmDeleted(DeserializeBody<Film>(message.Body));
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

        private void FilmCreated(Film createdFilm)
        {
            _logger.LogInformation("Message processed by \"{Processor}\" method", nameof(FilmCreated));

            createdFilm.ExternalId = createdFilm.Id;
            _filmsRepository.Create(createdFilm);

            _logger.LogInformation("Result: {Result}", JsonSerializer.Serialize(createdFilm));
        }

        private void FilmDeleted(Film deletedFilm)
        {
            _logger.LogInformation("Message processed by \"{Processor}\" method", nameof(FilmDeleted));

            if (_filmsRepository.GetByExternalId(deletedFilm.Id) is Film film)
            {
                _logger.LogInformation("Deleting film: FilmId: {Film}; Deleted: {IsDeleted}",
                    film.ExternalId, _filmsRepository.Delete(film));

                var ratings = _ratingsRepository.GetByFilmId(film.ExternalId);
                foreach (var rating in ratings)
                {
                    _logger.LogInformation("Deleting rating: FilmId:{FilmId}; UserId:{UserId}; Deleted: {IsDeleted}",
                        rating.FilmId, rating.UserId, _ratingsRepository.Delete(rating));
                }
            }
        }
    }
}