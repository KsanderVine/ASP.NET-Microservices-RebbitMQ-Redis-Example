using KinoSearch.Comments.Models;

namespace KinoSearch.Comments.Data
{
    public interface ICommentsRepository : IRepository<Models.Comment>
    {
        Comment? GetById(Guid userId, Guid filmId);
        IEnumerable<Comment> GetByUserId(Guid userId);
        IEnumerable<Comment> GetByFilmId(Guid filmId);
    }
}
