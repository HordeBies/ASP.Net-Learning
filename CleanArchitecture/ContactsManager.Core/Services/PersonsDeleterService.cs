using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;

namespace ContactsManager.Core.Services
{
    public class PersonsDeleterService : IPersonsDeleterService
    {
        private readonly IPersonsRepository personsRepository;
        public PersonsDeleterService(IPersonsRepository personsRepository)
        {
            this.personsRepository = personsRepository;
        }

        public async Task<bool> DeletePerson(Guid? PersonID)
        {
            if (PersonID == null || PersonID == Guid.Empty) return false;
            var person = await personsRepository.GetPerson(PersonID.Value);
            if (person == null) return false;
            await personsRepository.DeletePerson(PersonID.Value);
            return true;
        }
    }
}
