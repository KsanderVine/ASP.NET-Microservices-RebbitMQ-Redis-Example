using System.ComponentModel.DataAnnotations;

namespace KinoSearch.Comments.Dtos
{
    public class CommentCreateDto
    {
        [Required]
        public Guid FilmId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
