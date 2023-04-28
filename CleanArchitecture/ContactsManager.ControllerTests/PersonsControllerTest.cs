using AutoFixture;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.UI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContactsManager.ControllerTests
{
    public class PersonsControllerTest
    {
        private readonly Mock<IPersonsAdderService> personsAdderServiceMock;
        private readonly IPersonsAdderService personsAdderService;
        private readonly Mock<IPersonsDeleterService> personsDeleterServiceMock;
        private readonly IPersonsDeleterService personsDeleterService;
        private readonly Mock<IPersonsGetterService> personsGetterServiceMock;
        private readonly IPersonsGetterService personsGetterService;
        private readonly Mock<IPersonsSorterService> personsSorterServiceMock;
        private readonly IPersonsSorterService personsSorterService;
        private readonly Mock<IPersonsUpdaterService> personsUpdaterServiceMock;
        private readonly IPersonsUpdaterService personsUpdaterService;

        private readonly ICountriesService countriesService;
        private readonly ILogger<PersonsController> logger;

        private readonly Mock<ICountriesService> countriesServiceMock;
        private readonly Mock<ILogger<PersonsController>> loggerMock;

        private readonly IFixture fixture;
        public PersonsControllerTest()
        {
            fixture = new Fixture();

            personsAdderServiceMock = new();
            personsDeleterServiceMock = new();
            personsGetterServiceMock = new();
            personsSorterServiceMock = new();
            personsUpdaterServiceMock = new();

            personsAdderService = personsAdderServiceMock.Object;
            personsDeleterService = personsDeleterServiceMock.Object;
            personsGetterService = personsGetterServiceMock.Object;
            personsSorterService = personsSorterServiceMock.Object;
            personsUpdaterService = personsUpdaterServiceMock.Object;

            countriesServiceMock = new();
            loggerMock = new();

            countriesService = countriesServiceMock.Object;
            logger = loggerMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //Arrange
            List<PersonResponse> personResponses = fixture.Create<List<PersonResponse>>();

            PersonsController controller = new(countriesService, logger);

            personsGetterServiceMock.Setup(r => r.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(personResponses);
            personsSorterServiceMock.Setup(r => r.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(personResponses);

            //Act
            var result = await controller.Index(personsGetterService, personsSorterService, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<SortOrderOptions>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(personResponses);
        }
        #endregion
        #region Create
        [Fact]
        public async Task Create_ValidRequest()
        {
            PersonAddRequest personRequest = fixture.Create<PersonAddRequest>();
            PersonResponse personResponse = fixture.Create<PersonResponse>();
            List<CountryResponse> countries = fixture.Create<List<CountryResponse>>();

            PersonsController controller = new(countriesService, logger);

            countriesServiceMock.Setup(r => r.GetAllCountries()).ReturnsAsync(countries);
            personsAdderServiceMock.Setup(r => r.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

            //Act
            var result = await controller.Create(personsAdderService, personRequest);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        #endregion
        #region Edit
        [Fact]
        public async Task Edit_InvalidGetRequest()
        {
            List<CountryResponse> countries = fixture.Create<List<CountryResponse>>();

            PersonsController controller = new(countriesService, logger);

            personsGetterServiceMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(null as PersonResponse);
            countriesServiceMock.Setup(r => r.GetAllCountries()).ReturnsAsync(countries);

            var result = await controller.Edit(personsGetterService, Guid.Empty);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task Edit_ValidGetRequest()
        {
            PersonResponse personResponse = fixture.Build<PersonResponse>().With(r => r.Gender, "Male").Create();
            List<CountryResponse> countries = fixture.Create<List<CountryResponse>>();

            PersonsController controller = new(countriesService, logger);

            personsGetterServiceMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(personResponse);
            countriesServiceMock.Setup(r => r.GetAllCountries()).ReturnsAsync(countries);

            var result = await controller.Edit(personsGetterService, Guid.NewGuid());

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>().And.Be(personResponse.ToPersonUpdateRequest());
        }
        [Fact]
        public async Task Edit_ValidPostRequest()
        {
            PersonUpdateRequest personRequest = fixture.Create<PersonUpdateRequest>();
            PersonResponse personResponse = fixture.Create<PersonResponse>();
            List<CountryResponse> countries = fixture.Create<List<CountryResponse>>();

            PersonsController controller = new(countriesService, logger);

            personsUpdaterServiceMock.Setup(r => r.UpdatePerson(It.IsAny<PersonUpdateRequest>())).ReturnsAsync(personResponse);

            var result = await controller.Edit(personsUpdaterService, personRequest);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        #endregion
        #region Delete
        [Fact]
        public async Task Delete_InvalidGetRequest()
        {
            PersonsController controller = new(countriesService, logger);

            personsGetterServiceMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(null as PersonResponse);

            var result = await controller.Delete(personsGetterService, Guid.Empty);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task Delete_ValidGetRequest()
        {
            PersonResponse personResponse = fixture.Create<PersonResponse>();
            PersonsController controller = new(countriesService, logger);

            personsGetterServiceMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(personResponse);

            var result = await controller.Delete(personsGetterService, Guid.NewGuid());

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonResponse>().And.Be(personResponse);
        }
        [Fact]
        public async Task Delete_ValidPostRequest()
        {
            PersonResponse personResponse = fixture.Create<PersonResponse>();
            PersonsController controller = new(countriesService, logger);

            personsDeleterServiceMock.Setup(r => r.DeletePerson(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await controller.Delete(personsDeleterService, personResponse);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}
