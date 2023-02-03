using KinoSearch.Films.Models;

namespace KinoSearch.Films.Data
{
    public interface IFilmsRepository : IRepository<Guid, Film>
    {
    }
}
