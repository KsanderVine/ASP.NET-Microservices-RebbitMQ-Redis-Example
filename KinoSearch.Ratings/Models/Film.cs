namespace KinoSearch.Ratings.Models
{
    public class Film : BaseEntity
    {
        public Guid ExternalId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
