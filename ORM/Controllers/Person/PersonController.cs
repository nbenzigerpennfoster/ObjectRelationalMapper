using Domain;
using Microsoft.AspNetCore.Mvc;

namespace ORM.Controllers.Person
{
    [ApiController]
    public class PersonController : ControllerBase
    {
        public const string ROUTE = "api/person";
        public const string ROUTE_SINGLE = $"{ROUTE}/{{personid}}";

        private readonly IPersonRepository personRepository;
        private readonly IPersonAggregateRepository personAggregateRepository;
        private readonly IUnitOfWork unitOfWork;

        public PersonController(
            IPersonRepository personRepository,
            IPersonAggregateRepository personAggregateRepository,
            IUnitOfWork unitOfWork)
        {
            this.personRepository = personRepository;
            this.personAggregateRepository = personAggregateRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet(ROUTE)]
        public async Task<IActionResult> GetList()
        {
            var people = await personRepository.GetPeople(1, 10);
            var resource = people.Select(p => PersonResource.CreatePersonResource(p)).ToList();

            return Ok(resource);
        }

        [HttpGet(ROUTE_SINGLE)]
        public async Task<IActionResult> GetSingle(Guid personId)
        {
            var person = await personRepository.GetPerson(personId);
            if (person == null)
            {
                return NotFound();
            }

            var resource = PersonResource.CreatePersonResource(person);

            return Ok(resource);
        }

        [HttpPost(ROUTE)]
        public async Task<IActionResult> Post([FromBody] PersonResource personResource)
        {
            try
            {
                var personAddRequest = new Domain.Person
                {
                    FirstName = personResource.FirstName,
                    LastName = personResource.LastName,
                    Age = personResource.Age
                };
                if (personResource.Address != null)
                {
                    personAddRequest.Address = new Domain.Address
                    {
                        Address1 = personResource.Address.Address1,
                        Address2 = personResource.Address.Address2,
                        City = personResource.Address.City,
                        State = personResource.Address.State,
                        Zip = personResource.Address.Zip
                    };
                }

                var personAggregate = personAggregateRepository.GetPersonAggregate();

                personAggregate.AddPerson(personAddRequest);

                if (personAggregate.HasErrors())
                {
                    return BadRequest(personAggregate.Errors());
                }

                await unitOfWork.Commit(personAggregate);

                return Created(personAggregate.AggregateId, null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(ROUTE_SINGLE)]
        public async Task<IActionResult> Put(Guid personId, [FromBody] PersonResource personResource)
        {
            try
            {
                var personUpdateRequest = new Domain.Person
                {
                    FirstName = personResource.FirstName,
                    LastName = personResource.LastName,
                    Age = personResource.Age
                };
                if (personResource.Address != null)
                {
                    personUpdateRequest.Address = new Domain.Address
                    {
                        Address1 = personResource.Address.Address1,
                        Address2 = personResource.Address.Address2,
                        City = personResource.Address.City,
                        State = personResource.Address.State,
                        Zip = personResource.Address.Zip
                    };
                }

                var personAggregate = await personAggregateRepository.GetPersonAggregate(personId);
                if (personAggregate == null)
                {
                    return NotFound();
                }

                personAggregate.UpdatePerson(personUpdateRequest);

                if (personAggregate.HasErrors())
                {
                    return BadRequest(personAggregate.Errors());
                }

                await unitOfWork.Commit(personAggregate);

                return Ok();
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ROUTE_SINGLE)]
        public async Task<IActionResult> Delete(Guid personId)
        {
            var personAggregate = await personAggregateRepository.GetPersonAggregate(personId);
            if (personAggregate == null)
            {
                return NotFound();
            }

            personAggregate.DeletePerson();

            if (personAggregate.HasErrors())
            {
                return BadRequest(personAggregate.Errors());
            }

            await unitOfWork.Commit(personAggregate);

            return Ok();
        }
    }
}