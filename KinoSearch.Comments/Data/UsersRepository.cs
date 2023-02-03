using KinoSearch.Comments.Models;

namespace KinoSearch.Comments.Data
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
            return Users.Remove(entity);
        }

        public IEnumerable<User> GetAll()
        {
            return Users.ToList();
        }

        public User? GetByExternalId(Guid id)
        {
            return Users.FirstOrDefault(f => f.ExternalId == id);
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
