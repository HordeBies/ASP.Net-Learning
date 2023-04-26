using AutoFixture;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using FluentAssertions;
using Moq;

namespace ContactsManager.ServiceTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService countryService;
        private readonly ICountriesRepository countriesRepository;
        private readonly Mock<ICountriesRepository> countriesRepositoryMock;
        private readonly IFixture fixture;
        public CountriesServiceTest()
        {
            fixture = new Fixture();
            //var countries = new List<Country>();
            //var dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            //var dbContext = dbContextMock.Object;
            //dbContextMock.CreateDbSetMock(x => x.Countries, countries);
            countriesRepositoryMock = new();
            countriesRepository = countriesRepositoryMock.Object;

            countryService = new CountriesService(countriesRepository);
        }
        #region AddCountry
        [Fact]
        public async Task AddCountry_NullRequest()
        {
            var action = (async () => await countryService.AddCountry(null));
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task AddCountry_NullCountryName()
        {
            var countryAddRequest = fixture.Build<CountryAddRequest>().With(r => r.CountryName, null as string).Create();
            var action = (async () => await countryService.AddCountry(countryAddRequest));
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            var countryAddRequest = fixture.Create<CountryAddRequest>();
            countriesRepositoryMock.Setup(r => r.GetCountry(countryAddRequest.CountryName!)).ReturnsAsync(countryAddRequest.ToCountry());
            var action = (async () =>
            {
                await countryService.AddCountry(countryAddRequest);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task AddCountry_ValidRequest()
        {
            var countryAddRequest = fixture.Create<CountryAddRequest>();
            countriesRepositoryMock.Setup(r => r.AddCountry(It.IsAny<Country>())).ReturnsAsync((Country c) => { return c; });
            var countryResponse = await countryService.AddCountry(countryAddRequest);
            countryResponse.CountryName.Should().Be(countryAddRequest.CountryName);
            countryResponse.CountryID.Should().NotBeEmpty();
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            countriesRepositoryMock.Setup(r => r.GetAllCountries()).ReturnsAsync(new List<Country>());
            var actual = await countryService.GetAllCountries();
            actual.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllCountries_ValidRequest()
        {
            var expected = fixture.CreateMany<Country>(3);

            countriesRepositoryMock.Setup(r => r.GetAllCountries()).ReturnsAsync(expected.ToList());

            var actual = await countryService.GetAllCountries();

            actual.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region GetCountry
        [Fact]
        public async Task GetCountry_NullCountryID()
        {
            var countryResponse = await countryService.GetCountry(null);
            countryResponse.Should().BeNull();
        }
        [Fact]
        public async Task GetCountry_NonExistingCountryID()
        {
            var guid = Guid.NewGuid();
            countriesRepositoryMock.Setup(r => r.GetCountry(guid)).ReturnsAsync(null as Country);
            var countryResponse = await countryService.GetCountry(guid);
            countryResponse.Should().BeNull();
        }
        [Fact]
        public async Task GetCountry_ValidRequest()
        {
            var country = fixture.Create<Country>();
            countriesRepositoryMock.Setup(r => r.GetCountry(country.CountryID)).ReturnsAsync(country);
            var actual = await countryService.GetCountry(country.CountryID);
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(country);
        }
        #endregion
    }
}
