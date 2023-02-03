using KinoSearch.Ratings.Models;

namespace KinoSearch.Ratings.Data
{
    public class RatingsRepository : IRatingsRepository
    {
        private static List<Rating> Ratings { get; set; } = new List<Rating>();

        public void Create(Rating entity)
        {
            entity.CreatedAt = entity.UpdateAt = DateTime.UtcNow;
            Ratings.Add(entity);
        }

        public bool Delete(Rating entity)
        {
            return Ratings.Remove(entity);
        }

        public IEnumerable<Rating> GetAll()
        {
            return Ratings.ToList();
        }

        public IEnumerable<Rating> GetByFilmId(Guid filmId)
        {
            return Ratings.Where(r => r.FilmId == filmId);
        }

        public Rating? GetById(Guid userId, Guid filmId)
        {
            return Ratings.FirstOrDefault(r => r.UserId == userId && r.FilmId == filmId);
        }

        public IEnumerable<Rating> GetByUserId(Guid userId)
        {
            return Ratings.Where(r => r.UserId == userId);
        }

        public IEnumerable<Rating> GetWhere(Func<Rating, bool> predicate)
        {
            return Ratings.Where(predicate).ToList();
        }
    }
}
