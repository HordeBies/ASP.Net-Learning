using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
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

        public List<PersonResponse> GetFilteredPersons(string searchby, string? searchString)
        {
            var allPersons = GetPersons();
            List<PersonResponse> matchingPersons = new();
            if (string.IsNullOrEmpty(searchby) || string.IsNullOrEmpty(searchString))
                return allPersons;
            switch (searchby)
            {
                case "PersonName":
                    matchingPersons = allPersons.Where(p => p.PersonName != null && p.PersonName.Contains(searchString,StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "Address":
                    matchingPersons = allPersons.Where(p => p.Address != null && p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
                case "Country":
                    matchingPersons = allPersons.Where(p => p.Country != null && p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "Email":
                    matchingPersons = allPersons.Where(p => p.Email != null && p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "Age":
                    matchingPersons = allPersons.Where(p => p.Age != null && p.Age.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "DateOfBirth":
                    matchingPersons = allPersons.Where(p => p.DateOfBirth != null && p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "Gender":
                    matchingPersons = allPersons.Where(p => p.Gender != null && p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "ReceiveNewsLetters":
                    matchingPersons = allPersons.Where(p => p.ReceiveNewsLetters.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    matchingPersons = allPersons;
                    break;
            }
            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> collection, string sortby, SortOrder sortOrder)
        {
            if (string.IsNullOrEmpty(sortby))
                return collection;
            List<PersonResponse> sorted = sortby switch
            {
                "PersonName" => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.PersonName).ToList() : collection.OrderByDescending(i => i.PersonName).ToList(),
                "Address" => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Address).ToList() : collection.OrderByDescending(i => i.Address).ToList(),
                "Country" => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Country).ToList() : collection.OrderByDescending(i => i.Country).ToList(),
                "Email" => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Email).ToList() : collection.OrderByDescending(i => i.Email).ToList(),
                "Age" => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Age).ToList() : collection.OrderByDescending(i => i.Age).ToList(),
                "DateOfBirth" => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.DateOfBirth).ToList() : collection.OrderByDescending(i => i.DateOfBirth).ToList(),
                _ => collection
            };
            return sorted;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ValidationHelper.ModelValidation(request);
            var match = persons.FirstOrDefault(p => p.PersonID == request.PersonID);
            if (match == null) throw new ArgumentException("Given person does not exist");
            persons.Remove(match); //TODO: update in here instead of replacing it
            var person = request.ToPerson();
            persons.Add(person);
            return person.ToPersonResponse();
        }

        public bool DeletePerson(Guid? PersonID)
        {
            if (PersonID == null || PersonID == Guid.Empty) return false;
            var person = persons.FirstOrDefault(p => p.PersonID == PersonID);
            if (person == null) return false;
            return persons.Remove(person);
        }
    }
}
