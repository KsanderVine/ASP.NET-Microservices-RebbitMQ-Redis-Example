using KinoSearch.Films.Models;

namespace KinoSearch.Films.Data
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

        public Film? GetById(Guid @if)
        {
            return Films.FirstOrDefault(f => f.Id == @if);
        }

        public IEnumerable<Film> GetWhere(Func<Film, bool> predicate)
        {
            return Films.Where(predicate).ToList();
        }
    }
}
