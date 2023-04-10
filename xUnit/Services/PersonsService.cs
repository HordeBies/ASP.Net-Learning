using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private List<Person> persons;
        private ICountriesService countriesService;
        public PersonsService()
        {
            persons = new List<Person>();
            countriesService = new CountriesService();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            var response = person.ToPersonResponse();
            response.Country = countriesService.GetCountry(response.CountryID)?.CountryName;
            return response;
        }

        public PersonResponse AddPerson(PersonAddRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            ValidationHelper.ModelValidation(request);

            var person = request.ToPerson();
            person.PersonID = Guid.NewGuid();
            persons.Add(person);

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetPersons()
        {
            return persons.Select(p => ConvertPersonToPersonResponse(p)).ToList();
        }

        public PersonResponse? GetPerson(Guid? PersonID)
        {
            if (PersonID == null) return null;
            var person = persons.FirstOrDefault(p => p.PersonID == PersonID);
            if (person == null) return null;
            return ConvertPersonToPersonResponse(person);
        }

    }
}
