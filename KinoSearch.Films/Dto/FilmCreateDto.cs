using System.ComponentModel.DataAnnotations;

namespace KinoSearch.Films.Dto
{
    public class FilmCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
