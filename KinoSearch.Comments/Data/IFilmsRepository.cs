using KinoSearch.Comments.Models;

namespace KinoSearch.Comments.Data
{
    public interface IFilmsRepository : IRepository<Guid,Film>
    {
        Film? GetByExternalId(Guid id);
    }
}
