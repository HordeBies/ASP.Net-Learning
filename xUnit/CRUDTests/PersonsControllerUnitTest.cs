using AutoFixture;
using Moq;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using CRUDExample.Controllers;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsControllerUnitTest
    {
        private readonly IPersonsService personsService;
        private readonly ICountriesService countriesService;

        private readonly Mock<IPersonsService> personsServiceMock;
        private readonly Mock<ICountriesService> countriesServiceMock;
        private readonly Mock<ILogger<PersonsController>> loggerMock;

        private readonly IFixture fixture;
        public PersonsControllerUnitTest()
        {
            fixture = new Fixture();

            personsServiceMock = new();
            countriesServiceMock = new();
            loggerMock = new();

            personsService = personsServiceMock.Object;
            countriesService = countriesServiceMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //Arrange
            List<PersonResponse> personResponses = fixture.Create<List<PersonResponse>>();

            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            personsServiceMock.Setup(r => r.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(personResponses);
            personsServiceMock.Setup(r => r.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrder>())).ReturnsAsync(personResponses);

            //Act
            var result = await controller.Index(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(),fixture.Create<SortOrder>());

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

            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            countriesServiceMock.Setup(r => r.GetAllCountries()).ReturnsAsync(countries);
            personsServiceMock.Setup(r => r.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

            //Act
            var result = await controller.Create(personRequest);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task Create_InvalidRequest()
        {
            PersonAddRequest personRequest = fixture.Create<PersonAddRequest>();
            PersonResponse personResponse = fixture.Create<PersonResponse>();
            List<CountryResponse> countries = fixture.Create<List<CountryResponse>>();

            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            countriesServiceMock.Setup(r => r.GetAllCountries()).ReturnsAsync(countries);
            personsServiceMock.Setup(r => r.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

            //Act
            controller.ModelState.AddModelError("PersonName", "Person Name cannot be blank");
            var result = await controller.Create(personRequest);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>().And.Be(personRequest);
        }
        #endregion
        #region Edit
        [Fact]
        public async Task Edit_InvalidGetRequest()
        {
            List<CountryResponse> countries = fixture.Create<List<CountryResponse>>();

            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            personsServiceMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(null as PersonResponse);
            countriesServiceMock.Setup(r => r.GetAllCountries()).ReturnsAsync(countries);

            var result = await controller.Edit(Guid.Empty);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task Edit_ValidGetRequest()
        {
            PersonResponse personResponse = fixture.Build<PersonResponse>().With(r => r.Gender, "Male").Create();
            List<CountryResponse> countries = fixture.Create<List<CountryResponse>>();

            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            personsServiceMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(personResponse);
            countriesServiceMock.Setup(r => r.GetAllCountries()).ReturnsAsync(countries);

            var result = await controller.Edit(Guid.NewGuid());

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>().And.Be(personResponse.ToPersonUpdateRequest());
        }
        [Fact]
        public async Task Edit_ValidPostRequest()
        {
            PersonUpdateRequest personRequest= fixture.Create<PersonUpdateRequest>();
            PersonResponse personResponse = fixture.Create<PersonResponse>();
            List<CountryResponse> countries = fixture.Create<List<CountryResponse>>();

            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            personsServiceMock.Setup(r => r.UpdatePerson(It.IsAny<PersonUpdateRequest>())).ReturnsAsync(personResponse);

            var result = await controller.Edit(personRequest);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        #endregion
        #region Delete
        [Fact]
        public async Task Delete_InvalidGetRequest()
        {
            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            personsServiceMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(null as PersonResponse);

            var result = await controller.Delete(Guid.Empty);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task Delete_ValidGetRequest()
        {
            PersonResponse personResponse = fixture.Create<PersonResponse>();
            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            personsServiceMock.Setup(r => r.GetPerson(It.IsAny<Guid>())).ReturnsAsync(personResponse);

            var result = await controller.Delete(Guid.NewGuid());

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonResponse>().And.Be(personResponse);
        }
        [Fact]
        public async Task Delete_ValidPostRequest()
        {
            PersonResponse personResponse = fixture.Create<PersonResponse>();
            PersonsController controller = new(personsService, countriesService, loggerMock.Object);

            personsServiceMock.Setup(r => r.DeletePerson(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await controller.Delete(personResponse);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}
