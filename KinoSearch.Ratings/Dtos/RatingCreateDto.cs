using System.ComponentModel.DataAnnotations;

namespace KinoSearch.Ratings.Dtos
{
    public class RatingCreateDto
    {
        [Required]
        public Guid FilmId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Rate { get; set; }
    }
}
