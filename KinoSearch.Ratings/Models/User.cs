namespace KinoSearch.Ratings.Models
{
    public class User : BaseEntity
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }
}
