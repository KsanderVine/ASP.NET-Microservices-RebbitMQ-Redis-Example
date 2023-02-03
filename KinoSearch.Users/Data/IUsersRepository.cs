using KinoSearch.Users.Models;

namespace KinoSearch.Users.Data
{
    public interface IUsersRepository : IRepository<Guid, User>
    {
    }
}
