using KinoSearch.Comments.Data;
using KinoSearch.Comments.Models;
using Microsoft.AspNetCore.Mvc;

namespace KinoSearch.Comments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        // Only for demo reasons
        private readonly IUsersRepository _usersRepository;
        private readonly IFilmsRepository _filmsRepository;

        public ExternalController(
            IUsersRepository usersRepository,
            IFilmsRepository filmsRepository)
        {
            _usersRepository = usersRepository;
            _filmsRepository = filmsRepository;
        }

        [HttpGet("films", Name = nameof(GetAllFilms))]
        public ActionResult<IEnumerable<Film>> GetAllFilms()
        {
            return Ok(_filmsRepository.GetAll());
        }

        [HttpGet("users", Name = nameof(GetAllUsers))]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_usersRepository.GetAll());
        }
    }
}
