using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using System.Text;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService personsService;
        private readonly ICountriesService countriesService;
        public PersonsServiceTest()
        {
            personsService = new PersonsService();
            countriesService = new CountriesService();
        }
        #region AddPerson
        [Fact]
        public void AddPerson_NullRequest()
        {
            Assert.Throws<ArgumentNullException>(() => personsService.AddPerson(null));
        }
        [Fact]
        public void AddPerson_NullPersonName()
        {
            var personAddRequest = new PersonAddRequest
            {
                PersonName = null
            };
            Assert.Throws<ArgumentException>(() => personsService.AddPerson(personAddRequest));
        }
        [Fact]
        public void AddPerson_ValidRequest()
        {
            var personAddRequest = new PersonAddRequest
            {
                PersonName = "John",
                Address = "sample address",
                Email = "john@bies.com",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = new(2000,1,1),
                ReceieveNewsLetters = true
            };
            var expected = personsService.AddPerson(personAddRequest);
            var collection = personsService.GetPersons();
            Assert.True(expected.PersonID != Guid.Empty);
            Assert.Contains(expected, collection);
        }
        #endregion

        #region GetPerson
        [Fact]
        public void GetPerson_NullPersonID()
        {
            var expected = personsService.GetPerson(null);
            Assert.Null(expected);
        }
        [Fact]
        public void GetPerson_InvalidPersonID()
        {
            var expected = personsService.GetPerson(Guid.NewGuid());
            Assert.Null(expected);
        }
        [Fact]
        public void GetPerson_ValidPersonID()
        {
            var countryAddRequest = new CountryAddRequest
            {
                CountryName = "United States"
            };
            var countryResponse = countriesService.AddCountry(countryAddRequest);
            var personAddRequest = new PersonAddRequest
            {
                PersonName = "John",
                Email = "test@test.com",
                Address = "sample address",
                DateOfBirth = new(2000,6,12),
                Gender = GenderOptions.Other,
                ReceieveNewsLetters = true,
                CountryID = countryResponse.CountryID,
            };
            var expected = personsService.AddPerson(personAddRequest);
            var actual = personsService.GetPerson(expected.PersonID);
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}