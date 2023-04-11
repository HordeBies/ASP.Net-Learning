using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private List<Person> persons;
        private ICountriesService countriesService;
        public PersonsService(bool initialize = true)
        {
            persons = new List<Person>();
            countriesService = new CountriesService();
            if (initialize)
            {
                persons.Add(new Person
                {
                    PersonID = Guid.Parse("B763140E-FE26-495C-BF65-45FFEDB0076D"),
                    PersonName = "Chan Millery",
                    Email = "cmillery0@marriott.com",
                    DateOfBirth = new(1994,9,25),
                    Gender = "Male",
                    Address = "03 Mitchell Pass",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("AC1556E1-1811-4294-8049-D6C2352D2E73")
                });
                persons.Add(new Person
                {
                    PersonID = Guid.Parse("13D06051-2935-4B43-A750-32C81E4A7AF8"),
                    PersonName = "Euell Pelchat",
                    Email = "epelchat1@newsvine.com",
                    DateOfBirth = new(1957,2, 7),
                    Gender = "Male",
                    Address = "5 Myrtle Way",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("16F464AD-65D2-4E82-BD51-BDE0FEE91BB8")
                });
                persons.Add(new Person
                {
                    PersonID = Guid.Parse("5A854D38-D965-4586-9007-E30C500C395A"),
                    PersonName = "Thaxter Tomaselli",
                    Email = "ttomaselli2@networksolutions.com",
                    DateOfBirth = new(1969,7,14),
                    Gender = "Other",
                    Address = "28 Daystar Court",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("7DB3A34C-5B12-4558-93D2-997318B0E95A")
                });
                persons.Add(new Person
                {
                    PersonID = Guid.Parse("5C0D783F-C967-4BB3-95A9-CE51213C38F9"),
                    PersonName = "Washington Lindfors",
                    Email = "wlindfors3@creativecommons.org",
                    DateOfBirth = new(1989,6,17),
                    Gender = "Male",
                    Address = "55520 Acker Street",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("E839C350-555B-4847-A3A2-43517D543FC6")
                });
                persons.Add(new Person
                {
                    PersonID = Guid.Parse("A7C07FD9-89AE-4E6E-A351-10EFAC7853EA"),
                    PersonName = "Wilhelmine Trett",
                    Email = "wtrett4@blogspot.com",
                    DateOfBirth = new(1989,9,4),
                    Gender = "Female",
                    Address = "14 Center Court",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("2A638819-BEC1-4EEC-81EE-D864CEE07C46")
                });
                persons.Add(new Person
                {
                    PersonID = Guid.Parse("F9DCCC2A-64A2-4280-9C98-F38F12B1EED4"),
                    PersonName = "Roze Bushby",
                    Email = "rbushby5@dyndns.org",
                    DateOfBirth = new(1967,8,31),
                    Gender = "Female",
                    Address = "81 Barnett Court",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("7DB3A34C-5B12-4558-93D2-997318B0E95A")
                });
            }
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
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(p => p.PersonName != null && p.PersonName.Contains(searchString,StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(p => p.Address != null && p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
                case nameof(PersonResponse.Country):
                    matchingPersons = allPersons.Where(p => p.Country != null && p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(p => p.Email != null && p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Age):
                    matchingPersons = allPersons.Where(p => p.Age != null && p.Age.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(p => p.DateOfBirth != null && p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(p => p.Gender != null && p.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.ReceiveNewsLetters):
                    matchingPersons = allPersons.Where(p => p.ReceiveNewsLetters.ToString()!.Equals(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
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
                nameof(PersonResponse.PersonName) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.PersonName).ToList() : collection.OrderByDescending(i => i.PersonName).ToList(),
                nameof(PersonResponse.Address) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Address).ToList() : collection.OrderByDescending(i => i.Address).ToList(),
                nameof(PersonResponse.Country) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Country).ToList() : collection.OrderByDescending(i => i.Country).ToList(),
                nameof(PersonResponse.Email) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Email).ToList() : collection.OrderByDescending(i => i.Email).ToList(),
                nameof(PersonResponse.Age) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Age).ToList() : collection.OrderByDescending(i => i.Age).ToList(),
                nameof(PersonResponse.DateOfBirth) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.DateOfBirth).ToList() : collection.OrderByDescending(i => i.DateOfBirth).ToList(),
                nameof(PersonResponse.Gender) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Gender).ToList() : collection.OrderByDescending(i => i.Gender).ToList(),
                nameof(PersonResponse.ReceiveNewsLetters) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.ReceiveNewsLetters).ToList() : collection.OrderByDescending(i => i.ReceiveNewsLetters).ToList(),
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
