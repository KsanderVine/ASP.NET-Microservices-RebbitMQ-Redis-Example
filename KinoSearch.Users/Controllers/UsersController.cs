using KinoSearch.Users.Data;
using KinoSearch.Users.Models;
using KinoSearch.Users.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLib;

namespace KinoSearch.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMessageBusPublisher _publisher;
        private readonly IUsersRepository _repository;

        public UsersController(
            ILogger<UsersController> logger,
            IMessageBusPublisher publisher,
            IUsersRepository repository)
        {
            _logger = logger;
            _publisher = publisher;
            _repository = repository;
        }

        [HttpGet(Name = nameof(GetAll))]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        public ActionResult<User> GetById([FromRoute] Guid id)
        {
            var user = _repository.GetById(id);

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost(nameof(Create), Name = nameof(Create))]
        public ActionResult<User> Create([FromBody] UserCreateDto userCreate)
        {
            User user = new()
            {
                Name = userCreate.Name,
                Surname = userCreate.Surname
            };

            _repository.Create(user);

            _publisher.Publish(new RabbitMQMessage(user)
                .ToExchange("Data_Transfer_Topic")
                .WithRoutingKey("user.created"));

            return CreatedAtRoute(nameof(GetById), new { user.Id }, user);
        }

        [HttpDelete($"{nameof(Delete)}/{{id}}", Name = nameof(Delete))]
        public ActionResult Delete([FromRoute] Guid id)
        {
            var user = _repository.GetById(id);

            if (user is null)
                return NotFound();

            var result = _repository.Delete(user);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError);

            _publisher.Publish(new RabbitMQMessage(user)
                .ToExchange("Data_Transfer_Topic")
                .WithRoutingKey("user.deleted"));

            return Ok();
        }
    }
}
