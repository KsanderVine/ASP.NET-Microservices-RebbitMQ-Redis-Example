namespace KinoSearch.Ratings.Models
{
    public class Rating
    {
        public Guid FilmId { get; set; }
        public Guid UserId { get; set; }
        public int Rate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
