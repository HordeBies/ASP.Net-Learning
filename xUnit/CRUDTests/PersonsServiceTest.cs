using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using System.Text;
using Xunit.Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService personsService;
        private readonly ICountriesService countriesService;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly IFixture fixture;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            fixture = new Fixture();
            var countries = new List<Country>();
            var persons = new List<Person>();
            var dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            var dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(x => x.Countries, countries);
            dbContextMock.CreateDbSetMock(x => x.Persons, persons);
            countriesService = new CountriesService(dbContext);
            personsService = new PersonsService(dbContext);
        }
        #region AddPerson
        [Fact]
        public async Task AddPerson_NullRequest()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await personsService.AddPerson(null));
        }
        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            var personAddRequest = fixture.Build<PersonAddRequest>()
                .With(r => r.PersonName, null as string)
                .Create();
            await Assert.ThrowsAsync<ArgumentException>(async () => await personsService.AddPerson(personAddRequest));
        }
        [Fact]
        public async Task AddPerson_ValidRequest()
        {
            var personAddRequest = fixture.Build<PersonAddRequest>()
                .With(r => r.Email, "test@example.com")
                .Create();
            var expected = await personsService.AddPerson(personAddRequest);
            var collection = await personsService.GetPersons();
            Assert.True(expected.PersonID != Guid.Empty);
            Assert.Contains(expected, collection);
        }
        #endregion

        #region GetPerson
        [Fact]
        public async Task GetPerson_NullPersonID()
        {
            var expected = await personsService.GetPerson(null);
            Assert.Null(expected);
        }
        [Fact]
        public async Task GetPerson_InvalidPersonID()
        {
            var expected = await personsService.GetPerson(Guid.NewGuid());
            Assert.Null(expected);
        }
        [Fact]
        public async Task GetPerson_ValidPersonID()
        {
            var countryAddRequest = fixture.Create<CountryAddRequest>();
            var countryResponse = await countriesService.AddCountry(countryAddRequest);
            var personAddRequest = fixture.Build<PersonAddRequest>()
                .With(r => r.CountryID, countryResponse.CountryID)
                .With(r => r.Email, "test@example.com")
                .Create();
            var expected = await personsService.AddPerson(personAddRequest);
            var actual = await personsService.GetPerson(expected.PersonID);
            Assert.Equal(expected, actual);
        }
        #endregion
        private async Task<PersonResponse[]> CreatePersons()
        {
            var countryAddRequests = fixture.CreateMany<CountryAddRequest>(3);
            var countries = await Task.WhenAll(countryAddRequests.Select(async request => await countriesService.AddCountry(request)));
            var personAddRequests = new List<PersonAddRequest>
            {
                fixture.Build<PersonAddRequest>().With(r => r.Email, "email1@google.com").With(r => r.CountryID, countries[2].CountryID).With(r => r.PersonName, "Hasan Deniz").Create(),
                fixture.Build<PersonAddRequest>().With(r => r.Email, "email2@google.com").With(r => r.CountryID, countries[0].CountryID).Create(),
                fixture.Build<PersonAddRequest>().With(r => r.Email, "email3@google.com").With(r => r.CountryID, countries[1].CountryID).With(r => r.PersonName, "Hayri Türk").Create(),
            };
            return await Task.WhenAll(personAddRequests.Select(async request => await personsService.AddPerson(request)));
        }
        #region GetPersons
        [Fact]
        public async Task GetPersons_EmptyCollection()
        {
            var expected = await personsService.GetPersons();
            Assert.Empty(expected);
        }
        [Fact]
        public async Task GetPersons_ValidCollection()
        {
            var expected = await CreatePersons();

            var actual = await personsService.GetPersons();
            testOutputHelper.WriteLine("Expected:");
            foreach (var item in expected)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            testOutputHelper.WriteLine("Actual:");
            foreach (var item in actual)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            Assert.Equal(expected.Length, actual.Count);
            foreach (var person in expected)
            {
                Assert.Contains(person, actual);
            }
        }
        #endregion

        #region GetFilteredPersons
        //If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            var expected = await CreatePersons();

            var actual = await personsService.GetFilteredPersons(nameof(PersonResponse.PersonName), "");
            testOutputHelper.WriteLine("Expected:");
            foreach (var item in expected)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            testOutputHelper.WriteLine("Actual:");
            foreach (var item in actual)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            Assert.Equal(expected.Length, actual.Count);
            foreach (var person in expected)
            {
                Assert.Contains(person, actual);
            }
        }
        [Fact]
        public async Task GetFilteredPersons_ValidSearch()
        {
            var expected = await CreatePersons();

            var actual = await personsService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ha");
            testOutputHelper.WriteLine("Expected:");
            foreach (var item in expected)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            testOutputHelper.WriteLine("Actual:");
            foreach (var item in actual)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            foreach (var person in expected)
            {
                if (person.PersonName != null && person.PersonName.Contains("ha", StringComparison.OrdinalIgnoreCase))
                    Assert.Contains(person, actual);
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons_ValidSort()
        {
            var query = await CreatePersons();

            var actual = await personsService.GetSortedPersons(await personsService.GetPersons(), nameof(PersonResponse.PersonName), SortOrder.Descending);
            var expected = new List<PersonResponse>(query).OrderByDescending(i => i.PersonName).ToList();
            testOutputHelper.WriteLine("Expected:");
            foreach (var item in expected)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            testOutputHelper.WriteLine("Actual:");
            foreach (var item in actual)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            for (int idx = 0; idx < expected.Count; idx++)
            {
                Assert.Equal(expected[idx], actual[idx]);
            }
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_NullRequest()
        {
            PersonUpdateRequest? request = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await personsService.UpdatePerson(request));
        }
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            var request = fixture.Build<PersonUpdateRequest>()
                .With(r => r.Email, "test@example.com")
                .With(r => r.PersonID, Guid.NewGuid())
                .Create();

            await Assert.ThrowsAsync<ArgumentException>(async () => await personsService.UpdatePerson(request));
        }
        [Fact]
        public async Task UpdatePerson_NullPersonName()
        {
            var personAddRequest = fixture.Build<PersonAddRequest>().With(r => r.Email, "test@example.com").Create();
            var personResponse = await personsService.AddPerson(personAddRequest);
            var personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            await Assert.ThrowsAsync<ArgumentException>(async () => await personsService.UpdatePerson(personUpdateRequest));
        }
        [Fact]
        public async Task UpdatePerson_ValidPersonID()
        {
            var personAddRequest = fixture.Build<PersonAddRequest>().With(r => r.Email, "test@example.com").Create();
            var personResponse = await personsService.AddPerson(personAddRequest);

            var request = personResponse.ToPersonUpdateRequest();
            request.PersonName = "Mehmet Demirci";
            request.Email = "oa.mehmetdmrc@gmail.com";

            var expected = await personsService.UpdatePerson(request);
            var actual = await personsService.GetPerson(request.PersonID);

            Assert.Equal(expected, actual);
        }
        #endregion

        #region DeletePerson
        [Fact]
        public async Task DeletePerson_NonExistentID()
        {
            Guid nonExistentID = Guid.NewGuid();

            bool result = await personsService.DeletePerson(nonExistentID);

            Assert.False(result);
        }

        [Fact]
        public async Task DeletePerson_ValidID()
        {
            var personAddRequest = fixture.Build<PersonAddRequest>().With(r => r.Email, "test@example.com").Create();
            var personResponse = await personsService.AddPerson(personAddRequest);

            bool result = await personsService.DeletePerson(personResponse.PersonID);

            var expected = await personsService.GetPerson(personResponse.PersonID);

            Assert.True(result);
            Assert.Null(expected);
        }
        #endregion
    }
}