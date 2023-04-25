using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using RepositoryContracts;

namespace Services
{
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsRepository personsRepository;
        public PersonsAdderService(IPersonsRepository personsRepository)
        {
            this.personsRepository = personsRepository;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            ValidationHelper.ModelValidation(request);

            var person = request.ToPerson();
            person.PersonID = Guid.NewGuid();
            await personsRepository.AddPerson(person);
            //db.sp_InsertPerson(person);

            return person.ToPersonResponse();
        }
    }
}
