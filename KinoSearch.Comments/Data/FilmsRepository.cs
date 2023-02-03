using KinoSearch.Comments.Models;

namespace KinoSearch.Comments.Data
{
    public class FilmsRepository : IFilmsRepository
    {
        private static List<Film> Films { get; set; } = new List<Film>();

        public void Create(Film entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = entity.UpdateAt = DateTime.UtcNow;
            Films.Add(entity);
        }

        public bool Delete(Film entity)
        {
            return Films.Remove(entity);
        }

        public IEnumerable<Film> GetAll()
        {
            return Films.ToList();
        }

        public Film? GetByExternalId(Guid id)
        {
            return Films.FirstOrDefault(f => f.ExternalId == id);
        }

        public Film? GetById(Guid id)
        {
            return Films.FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Film> GetWhere(Func<Film, bool> predicate)
        {
            return Films.Where(predicate).ToList();
        }
    }
}
