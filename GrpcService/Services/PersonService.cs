using Grpc.Core;
using GrpcService.data;
using GrpcService.Protos;

namespace GrpcService.Services
{
    public class PersonService : PersonDetails.PersonDetailsBase
    {
        PersonRepository personRepo = new PersonRepository();

        public override Task<PersonListResponse> GetPerson(PersonRequest request, ServerCallContext context)
        {
            var persons = personRepo.GetPerson(request.Id);

            var personResponse = new List<PersonResponse>();
            foreach (var person in persons)
            {
                personResponse.Add(new PersonResponse
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    NationalCode = person.NationalCode,
                    BirthDate = person.BirthDate,
                });
            }

            var personListResponse = new PersonListResponse
            {
               Person = { personResponse }
            };

            return Task.FromResult(personListResponse);

        }
    }
}
