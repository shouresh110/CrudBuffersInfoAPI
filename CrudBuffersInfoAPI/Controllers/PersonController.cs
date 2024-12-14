using GrpcService.data;
using GrpcService.Protos;
using GrpcService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudBuffersInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        PersonService personService = new PersonService();
        private readonly PersonRepository _repository = new();

        [HttpPost("Create")]
        public IActionResult Create([FromBody] Person person)
        {
            _repository.Create(person);
            return Ok();
        }

        // Implemented with Protocol buffers
        [HttpGet("Read")]
        public PersonListResponse GetPersonList(int id)
        {
            PersonRequest personRequest = new PersonRequest();
            personRequest.Id = id;
            return personService.GetPerson(personRequest, null).Result;
        }

        [HttpPut("Update")]
        public IActionResult Update(int id, [FromBody] Person updatedPerson)
        {
            if (id != updatedPerson.Id) return BadRequest();

            var success = _repository.Update(updatedPerson);
            if (!success) return NotFound();

            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var success = _repository.Delete(id);
            if (!success) return NotFound();

            return Ok();
        }
    }
}
