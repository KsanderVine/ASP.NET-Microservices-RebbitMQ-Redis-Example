using KinoSearch.Users.Models;

namespace KinoSearch.Users.Data
{
    public class UsersRepository : IUsersRepository
    {
        private static List<User> Users { get; set; } = new List<User>();

        public void Create(User entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = entity.UpdateAt = DateTime.UtcNow;
            Users.Add(entity);
        }

        public bool Delete(User entity)
        {
            /*if(GetById(entity.Id) is User foundEntity)
                return Users.Remove(entity);
            return false;*/
            return Users.Remove(entity);
        }

        public IEnumerable<User> GetAll()
        {
            return Users.ToList();
        }

        public User? GetById(Guid id)
        {
            return Users.FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<User> GetWhere(Func<User, bool> predicate)
        {
            return Users.Where(predicate).ToList();
        }
    }
}
