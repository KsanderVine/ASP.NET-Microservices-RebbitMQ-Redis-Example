using KinoSearch.Comments.Data;
using KinoSearch.Comments.Dtos;
using KinoSearch.Comments.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KinoSearch.Comments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IFilmsRepository _filmsRepository;
        private readonly ICommentsRepository _commentsRepository;

        public CommentsController(
            ILogger<CommentsController> logger,
            IUsersRepository usersRepository,
            IFilmsRepository filmsRepository,
            ICommentsRepository commentsRepository)
        {
            _logger = logger;
            _commentsRepository = commentsRepository;
            _usersRepository = usersRepository;
            _filmsRepository = filmsRepository;
        }

        [HttpGet(Name = nameof(GetAll))]
        public ActionResult<IEnumerable<Comment>> GetAll()
        {
            return Ok(_commentsRepository.GetAll());
        }

        [HttpGet("{filmId}/{userId}", Name = nameof(GetById))]
        public ActionResult<Comment> GetById([FromRoute] Guid filmId, [FromRoute] Guid userId)
        {
            var rating = _commentsRepository.GetById(userId, filmId);

            if (rating is null)
                return NotFound();

            return Ok(rating);
        }

        [HttpPost(nameof(Create), Name = nameof(Create))]
        public ActionResult<Comment> Create(
            [FromBody] CommentCreateDto createDto)
        {
            var film = _filmsRepository.GetByExternalId(createDto.FilmId);
            var user = _usersRepository.GetByExternalId(createDto.UserId);

            if (film is null || user is null)
            {
                return BadRequest();
            }

            Comment comment = new()
            {
                UserId = createDto.UserId,
                FilmId = createDto.FilmId,
                Text = createDto.Text
            };

            _commentsRepository.Create(comment);

            return CreatedAtRoute(nameof(GetById), new { comment.FilmId, comment.UserId }, comment);
        }

        [HttpDelete($"{nameof(Delete)}", Name = nameof(Delete))]
        public ActionResult Delete([FromBody] CommentDeleteDto deleteDto)
        {
            var comment = _commentsRepository.GetById(deleteDto.UserId, deleteDto.FilmId);

            if (comment is null)
                return NotFound();

            var result = _commentsRepository.Delete(comment);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
