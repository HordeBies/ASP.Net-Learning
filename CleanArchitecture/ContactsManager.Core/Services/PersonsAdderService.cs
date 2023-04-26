using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.ServiceContracts;

namespace ContactsManager.Core.Services
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
