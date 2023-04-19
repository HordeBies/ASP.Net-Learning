using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;
using System.Globalization;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService personsService;

        private readonly Mock<IPersonsRepository> personsRepositoryMock;
        private readonly IPersonsRepository personsRepository;

        private readonly ITestOutputHelper testOutputHelper;
        private readonly IFixture fixture;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            fixture = new Fixture();

            personsRepositoryMock = new();
            personsRepository = personsRepositoryMock.Object;

            //var countries = new List<Country>();
            //var persons = new List<Person>();
            //var dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            //var dbContext = dbContextMock.Object;
            //dbContextMock.CreateDbSetMock(x => x.Countries, countries);
            //dbContextMock.CreateDbSetMock(x => x.Persons, persons);
            personsService = new PersonsService(personsRepository);
        }
        #region AddPerson
        [Fact]
        public async Task AddPerson_NullRequest_ToBeArgumentNullException()
        {
            var action = (async () => await personsService.AddPerson(null));
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task AddPerson_NullPersonName_ToBeArgumentException()
        {
            var personAddRequest = fixture.Build<PersonAddRequest>()
                .With(r => r.PersonName, null as string)
                .Create();
            var person = personAddRequest.ToPerson();
            personsRepositoryMock.Setup(r => r.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            var action = (async () => await personsService.AddPerson(personAddRequest));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            var personAddRequest = fixture.Build<PersonAddRequest>()
                .With(r => r.Email, "test@example.com")
                .Create();
            var person = personAddRequest.ToPerson();
            personsRepositoryMock.Setup(r => r.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            var exptected = person.ToPersonResponse();

            //Act
            var actual = await personsService.AddPerson(personAddRequest);
            
            exptected.PersonID = actual.PersonID;

            //Assert
            actual.Should().NotBeNull();
            actual.PersonID.Should().NotBeEmpty();
            actual.Should().Be(exptected);
        }
        #endregion

        #region GetPerson
        [Fact]
        public async Task GetPerson_NullPersonID_ToBeNull()
        {
            var expected = await personsService.GetPerson(null);
            expected.Should().BeNull();
        }
        [Fact]
        public async Task GetPerson_InvalidPersonID_ToBeNull()
        {
            var guid = Guid.NewGuid();
            personsRepositoryMock.Setup(r => r.GetPerson(guid)).ReturnsAsync(null as Person);
            var expected = await personsService.GetPerson(guid);
            expected.Should().BeNull();
        }
        [Fact]
        public async Task GetPerson_ValidPersonID()
        {
            var person = fixture.Build<Person>()
                .With(r => r.Email, "test@example.com")
                .With(r => r.Country, null as Country)
                .Create();
            personsRepositoryMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(person);
            var expected = person.ToPersonResponse();

            var actual = await personsService.GetPerson(person.PersonID);
            actual.Should().Be(expected);
        }
        #endregion
        private List<Person> CreatePersons()
        {
            var persons = new List<Person>
            {
                fixture.Build<Person>()
                .With(r => r.Email, "email1@google.com")
                .With(r => r.PersonName, "Hasan Deniz")
                .With(r => r.Gender, "Male")
                .With(r => r.Country, null as Country)
                .Create(),
                fixture.Build < Person >()
                .With(r => r.Email, "email2@google.com")
                .With(r => r.Gender, "Male")
                .With(r => r.Country, null as Country)
                .Create(),
                fixture.Build < Person >()
                .With(r => r.Email, "email3@google.com")
                .With(r => r.PersonName, "Hayri Türk")
                .With(r => r.Gender, "Male")
                .With(r => r.Country, null as Country)
                .Create(),
            };
            return persons;
        }
        #region GetAllPersons
        [Fact]
        public async Task GetAllPersons_ToBeEmptyCollection()
        {
            personsRepositoryMock.Setup(r => r.GetAllPersons()).ReturnsAsync(new List<Person>());
            var expected = await personsService.GetAllPersons();
            expected.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            var personList = CreatePersons();
            personsRepositoryMock.Setup(r => r.GetAllPersons()).ReturnsAsync(personList);
            var expected = personList.Select(p => p.ToPersonResponse()).ToList();
            var actual = await personsService.GetAllPersons();

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

            actual.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region GetFilteredPersons
        //If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            var personList = CreatePersons();
            personsRepositoryMock.Setup(r => r.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(personList);
            var expected = personList.Select(p => p.ToPersonResponse()).ToList();
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

            actual.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task GetFilteredPersons_ValidSearch()
        {
            var personList = CreatePersons();
            personsRepositoryMock.Setup(r => r.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(personList.Where(p => p.PersonName != null && p.PersonName.Contains("ha",StringComparison.OrdinalIgnoreCase)).ToList());
            var expected = personList.Select(p => p.ToPersonResponse()).ToList();

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

            actual.Should().OnlyContain(expected => expected.PersonName!.Contains("ha",StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons_ValidSort()
        {
            var personList = CreatePersons();
            personsRepositoryMock.Setup(r => r.GetAllPersons()).ReturnsAsync(personList);
            var expected = personList.Select(p => p.ToPersonResponse()).ToList();

            var actual = await personsService.GetSortedPersons(await personsService.GetAllPersons(), nameof(PersonResponse.PersonName), SortOrder.Descending);

            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            actual.Should().BeInDescendingOrder(expected => expected.PersonName, comparer:comparer);
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_NullRequest_ToBeArgumentNullException()
        {
            PersonUpdateRequest? request = null;
            var action = (async () => await personsService.UpdatePerson(request));
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            var request = fixture.Build<Person>()
                .With(r => r.Email, "test@example.com")
                .With(r => r.Gender, "Male")
                .With(r => r.Country, null as Country)
                .Create();
            personsRepositoryMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(null as Person);
            var action = (async () => await personsService.UpdatePerson(request.ToPersonResponse().ToPersonUpdateRequest()));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task UpdatePerson_NullPersonName()
        {
            var person = fixture.Build<Person>()
                .With(r => r.Email, "test@example.com")
                .With(r => r.Gender, "Male")
                .With(r => r.Country, null as Country)
                .Create();
            var personResponse = person.ToPersonResponse();
            var personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            var action = (async () => await personsService.UpdatePerson(personUpdateRequest));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task UpdatePerson_ValidPersonID()
        {
            var person = fixture.Build<Person>()
                .With(r => r.Email, "test@example.com")
                .With(r => r.Gender, "Male")
                .With(r => r.Country, null as Country)
                .Create();
            var personResponse = person.ToPersonResponse();
            var request = personResponse.ToPersonUpdateRequest();

            request.PersonName = "Mehmet Demirci";
            request.Email = "oa.mehmetdmrc@gmail.com";
            var updatedPerson = request.ToPerson();
            personsRepositoryMock.Setup(r => r.GetPerson(person.PersonID)).ReturnsAsync(person);
            personsRepositoryMock.Setup(r => r.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(updatedPerson);
            var expected = updatedPerson.ToPersonResponse();

            var actual = await personsService.UpdatePerson(request);

            actual.Should().Be(expected);
        }
        #endregion

        #region DeletePerson
        [Fact]
        public async Task DeletePerson_NonExistentID()
        {
            Guid nonExistentID = Guid.NewGuid();

            bool result = await personsService.DeletePerson(nonExistentID);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeletePerson_ValidID()
        {
            var person = fixture.Build<Person>()
                .With(r => r.Email, "test@example.com")
                .With(r => r.Country, null as Country)
                .With(r => r.Gender, "Male")
                .Create();

            personsRepositoryMock.Setup(r => r.GetPerson(person.PersonID)).ReturnsAsync(person);
            personsRepositoryMock.Setup(r => r.DeletePerson(person.PersonID)).ReturnsAsync(true);

            bool result = await personsService.DeletePerson(person.PersonID);

            result.Should().BeTrue();
        }
        #endregion
    }
}