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
        private PersonsDbContext db;
        private ICountriesService countriesService;
        public PersonsService(PersonsDbContext personsDbContext, ICountriesService countriesService)
        {
            db = personsDbContext;
            this.countriesService = countriesService; 
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
            //db.Persons.Add(person);
            //db.SaveChanges();
            db.sp_InsertPerson(person);

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetPersons()
        {
            return db.sp_GetAllPersons().Select(p => ConvertPersonToPersonResponse(p)).ToList();
        }

        public PersonResponse? GetPerson(Guid? PersonID)
        {
            if (PersonID == null) return null;
            var person = db.Persons.FirstOrDefault(p => p.PersonID == PersonID);
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
            var match = db.Persons.FirstOrDefault(p => p.PersonID == request.PersonID);
            if (match == null) throw new ArgumentException("Given person does not exist");
            match.PersonName = request.PersonName;
            match.Address = request.Address;
            match.CountryID = request.CountryID;
            match.DateOfBirth = request.DateOfBirth;
            match.Email = request.Email;
            match.Gender = request.Gender.ToString();
            match.ReceiveNewsLetters = request.ReceiveNewsLetters;
            db.SaveChanges();
            return match.ToPersonResponse();
        }

        public bool DeletePerson(Guid? PersonID)
        {
            if (PersonID == null || PersonID == Guid.Empty) return false;
            var person = db.Persons.FirstOrDefault(p => p.PersonID == PersonID);
            if (person == null) return false;
            db.Remove(person);
            db.SaveChanges();
            return true;
        }
    }
}
