using KinoSearch.Comments.Models;

namespace KinoSearch.Comments.Data
{
    public class CommentsRepository : ICommentsRepository
    {
        private static List<Comment> Comments { get; set; } = new List<Comment>();

        public void Create(Comment entity)
        {
            entity.CreatedAt = entity.UpdateAt = DateTime.UtcNow;
            Comments.Add(entity);
        }

        public bool Delete(Comment entity)
        {
            return Comments.Remove(entity);
        }

        public IEnumerable<Comment> GetAll()
        {
            return Comments.ToList();
        }

        public IEnumerable<Comment> GetByFilmId(Guid filmId)
        {
            return Comments.Where(r => r.FilmId == filmId);
        }

        public Comment? GetById(Guid userId, Guid filmId)
        {
            return Comments.FirstOrDefault(r => r.UserId == userId && r.FilmId == filmId);
        }

        public IEnumerable<Comment> GetByUserId(Guid userId)
        {
            return Comments.Where(r => r.UserId == userId);
        }

        public IEnumerable<Comment> GetWhere(Func<Comment, bool> predicate)
        {
            return Comments.Where(predicate).ToList();
        }
    }
}
