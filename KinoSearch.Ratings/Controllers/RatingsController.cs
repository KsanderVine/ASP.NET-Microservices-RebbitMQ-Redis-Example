using KinoSearch.Ratings.Data;
using KinoSearch.Ratings.Dtos;
using KinoSearch.Ratings.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KinoSearch.Ratings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly ILogger<RatingsController> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IFilmsRepository _filmsRepository;
        private readonly IRatingsRepository _ratingsRepository;

        public RatingsController(
            ILogger<RatingsController> logger,
            IUsersRepository usersRepository,
            IFilmsRepository filmsRepository,
            IRatingsRepository ratingsRepository)
        {
            _logger = logger;
            _ratingsRepository = ratingsRepository;
            _usersRepository = usersRepository;
            _filmsRepository = filmsRepository;
        }


        [HttpGet(Name = nameof(GetAll))]
        public ActionResult<IEnumerable<Rating>> GetAll()
        {
            return Ok(_ratingsRepository.GetAll());
        }

        [HttpGet("{filmId}/{userId}", Name = nameof(GetById))]
        public ActionResult<Rating> GetById([FromRoute] Guid filmId, [FromRoute] Guid userId)
        {
            var rating = _ratingsRepository.GetById(userId, filmId);

            if (rating is null)
                return NotFound();

            return Ok(rating);
        }

        [HttpPost(nameof(Create), Name = nameof(Create))]
        public ActionResult<User> Create(
            [FromBody] RatingCreateDto createDto)
        {
            var film = _filmsRepository.GetByExternalId(createDto.FilmId);
            var user = _usersRepository.GetByExternalId(createDto.UserId);

            if (film is null || user is null)
            {
                return BadRequest();
            }

            Rating rating = new()
            {
                UserId = createDto.UserId,
                FilmId = createDto.FilmId,
                Rate = createDto.Rate
            };

            _ratingsRepository.Create(rating);

            return CreatedAtRoute(nameof(GetById), new { rating.FilmId, rating.UserId }, rating);
        }

        [HttpDelete($"{nameof(Delete)}", Name = nameof(Delete))]
        public ActionResult Delete([FromBody] RatingDeleteDto deleteDto)
        {
            var rating = _ratingsRepository.GetById(deleteDto.UserId, deleteDto.FilmId);

            if (rating is null)
                return NotFound();

            var result = _ratingsRepository.Delete(rating);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
