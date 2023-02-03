using KinoSearch.Films.Data;
using KinoSearch.Films.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using RabbitMQLib;
using KinoSearch.Films.Dto;

namespace KinoSearch.Films.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly ILogger<FilmsController> _logger;
        private readonly IMessageBusPublisher _publisher;
        private readonly IFilmsRepository _repository;

        public FilmsController(
            ILogger<FilmsController> logger,
            IMessageBusPublisher publisher,
            IFilmsRepository repository)
        {
            _logger = logger;
            _publisher = publisher;
            _repository = repository;
        }

        [HttpGet(Name = nameof(GetAll))]
        public ActionResult<IEnumerable<Film>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        public ActionResult<Film> GetById([FromRoute]Guid id)
        {
            var film = _repository.GetById(id);

            if (film is null)
                return NotFound();

            return Ok(film);
        }

        [HttpPost(nameof(Create), Name = nameof(Create))]
        public ActionResult<Film> Create([FromBody] FilmCreateDto createDto)
        {
            Film film = new() { Title = createDto.Title };

            _repository.Create(film);

            _publisher.Publish(new RabbitMQMessage(film)
                .ToExchange("Data_Transfer_Topic")
                .WithRoutingKey("film.created"));

            return CreatedAtRoute(nameof(GetById), new { film.Id }, film);
        }

        [HttpDelete($"{nameof(Delete)}/{{id}}", Name = nameof(Delete))]
        public ActionResult Delete([FromRoute] Guid id)
        {
            var film = _repository.GetById(id);
            if(film is null)
                return NotFound();

            var result = _repository.Delete(film);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError);

            _publisher.Publish(new RabbitMQMessage(film)
                .ToExchange("Data_Transfer_Topic")
                .WithRoutingKey("film.deleted"));

            return Ok();
        }
    }
}
