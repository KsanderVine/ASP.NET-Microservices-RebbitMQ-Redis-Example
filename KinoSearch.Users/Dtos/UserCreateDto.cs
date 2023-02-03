using System.ComponentModel.DataAnnotations;

namespace KinoSearch.Users.Dtos
{
    public class UserCreateDto
    { 
        [Required] 
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;
    }
}