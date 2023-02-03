using KinoSearch.Comments.Models;

namespace KinoSearch.Comments.Data
{
    public interface IUsersRepository : IRepository<Guid, User>
    {
        User? GetByExternalId(Guid id);
    }
}
