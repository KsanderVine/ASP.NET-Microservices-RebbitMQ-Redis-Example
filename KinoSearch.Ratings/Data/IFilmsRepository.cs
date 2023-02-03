using KinoSearch.Ratings.Models;

namespace KinoSearch.Ratings.Data
{
    public interface IFilmsRepository : IRepository<Guid,Film>
    {
        Film? GetByExternalId(Guid id);
    }
}
