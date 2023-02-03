using System.ComponentModel.DataAnnotations;

namespace KinoSearch.Comments.Dtos
{
    public class CommentDeleteDto
    {
        [Required]
        public Guid FilmId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
