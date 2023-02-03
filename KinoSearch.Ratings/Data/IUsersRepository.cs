using KinoSearch.Ratings.Models;

namespace KinoSearch.Ratings.Data
{
    public interface IUsersRepository : IRepository<Guid, User>
    {
        User? GetByExternalId(Guid id);
    }
}
