namespace KinoSearch.Comments.Models
{
    public class Comment
    {
        public Guid FilmId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
