using System.ComponentModel.DataAnnotations;

namespace KinoSearch.Ratings.Dtos
{
    public class RatingDeleteDto
    {
        [Required]
        public Guid FilmId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
