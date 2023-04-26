using AutoFixture;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using System.Globalization;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace ContactsManager.ServiceTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsAdderService personsAdderService;
        private readonly IPersonsDeleterService personsDeleterService;
        private readonly IPersonsGetterService personsGetterService;
        private readonly IPersonsSorterService personsSorterService;
        private readonly IPersonsUpdaterService personsUpdaterService;

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

            personsAdderService = new PersonsAdderService(personsRepository);
            personsDeleterService = new PersonsDeleterService(personsRepository);
            personsGetterService = new PersonsGetterService(personsRepository, new Mock<ILogger<PersonsGetterService>>().Object, new Mock<IDiagnosticContext>().Object);
            personsSorterService = new PersonsSorterService(new Mock<ILogger<PersonsSorterService>>().Object);
            personsUpdaterService = new PersonsUpdaterService(personsRepository);
        }
        #region AddPerson
        [Fact]
        public async Task AddPerson_NullRequest_ToBeArgumentNullException()
        {
            var action = (async () => await personsAdderService.AddPerson(null));
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

            var action = (async () => await personsAdderService.AddPerson(personAddRequest));
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
            var actual = await personsAdderService.AddPerson(personAddRequest);

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
            var expected = await personsGetterService.GetPerson(null);
            expected.Should().BeNull();
        }
        [Fact]
        public async Task GetPerson_InvalidPersonID_ToBeNull()
        {
            var guid = Guid.NewGuid();
            personsRepositoryMock.Setup(r => r.GetPerson(guid)).ReturnsAsync(null as Person);
            var expected = await personsGetterService.GetPerson(guid);
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

            var actual = await personsGetterService.GetPerson(person.PersonID);
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
            var expected = await personsGetterService.GetAllPersons();
            expected.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            var personList = CreatePersons();
            personsRepositoryMock.Setup(r => r.GetAllPersons()).ReturnsAsync(personList);
            var expected = personList.Select(p => p.ToPersonResponse()).ToList();
            var actual = await personsGetterService.GetAllPersons();

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
            personsRepositoryMock.Setup(r => r.GetAllPersons()).ReturnsAsync(personList);
            var expected = personList.Select(p => p.ToPersonResponse()).ToList();
            var actual = await personsGetterService.GetFilteredPersons(nameof(PersonResponse.PersonName), "");
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
            personsRepositoryMock.Setup(r => r.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(personList.Where(p => p.PersonName != null && p.PersonName.Contains("ha", StringComparison.OrdinalIgnoreCase)).ToList());
            var expected = personList.Select(p => p.ToPersonResponse()).ToList();

            var actual = await personsGetterService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ha");
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

            actual.Should().OnlyContain(expected => expected.PersonName!.Contains("ha", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons_ValidSort()
        {
            var personList = CreatePersons();
            personsRepositoryMock.Setup(r => r.GetAllPersons()).ReturnsAsync(personList);
            var expected = personList.Select(p => p.ToPersonResponse()).ToList();

            var actual = await personsSorterService.GetSortedPersons(await personsGetterService.GetAllPersons(), nameof(PersonResponse.PersonName), SortOrder.Descending);

            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            actual.Should().BeInDescendingOrder(expected => expected.PersonName, comparer: comparer);
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_NullRequest_ToBeArgumentNullException()
        {
            PersonUpdateRequest? request = null;
            var action = (async () => await personsUpdaterService.UpdatePerson(request));
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
            var action = (async () => await personsUpdaterService.UpdatePerson(request.ToPersonResponse().ToPersonUpdateRequest()));
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

            var action = (async () => await personsUpdaterService.UpdatePerson(personUpdateRequest));
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

            var actual = await personsUpdaterService.UpdatePerson(request);

            actual.Should().Be(expected);
        }
        #endregion

        #region DeletePerson
        [Fact]
        public async Task DeletePerson_NonExistentID()
        {
            Guid nonExistentID = Guid.NewGuid();

            bool result = await personsDeleterService.DeletePerson(nonExistentID);

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

            bool result = await personsDeleterService.DeletePerson(person.PersonID);

            result.Should().BeTrue();
        }
        #endregion
    }
}