using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Exceptions;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.ServiceContracts;

namespace ContactsManager.Core.Services
{
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        private readonly IPersonsRepository personsRepository;
        public PersonsUpdaterService(IPersonsRepository personsRepository)
        {
            this.personsRepository = personsRepository;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ValidationHelper.ModelValidation(request);
            var match = await personsRepository.GetPerson(request.PersonID);
            if (match == null) throw new InvalidPersonIDException("Given person does not exist");
            match.PersonName = request.PersonName;
            match.Address = request.Address;
            match.CountryID = request.CountryID;
            match.DateOfBirth = request.DateOfBirth;
            match.Email = request.Email;
            match.Gender = request.Gender.ToString();
            match.ReceiveNewsLetters = request.ReceiveNewsLetters;
            match = await personsRepository.UpdatePerson(match);
            return match.ToPersonResponse();
        }
    }
}
