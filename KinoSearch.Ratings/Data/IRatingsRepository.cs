using KinoSearch.Ratings.Models;

namespace KinoSearch.Ratings.Data
{
    public interface IRatingsRepository : IRepository<Rating>
    {
        Rating? GetById(Guid userId, Guid filmId);
        IEnumerable<Rating> GetByUserId(Guid userId);
        IEnumerable<Rating> GetByFilmId(Guid filmId);
    }
}
