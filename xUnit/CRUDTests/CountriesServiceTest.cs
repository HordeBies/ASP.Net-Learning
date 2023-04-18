using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService countryService;
        private readonly IFixture fixture;
        public CountriesServiceTest()
        {
            fixture = new Fixture();
            var countries = new List<Country>();
            var dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            var dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(x => x.Countries, countries);
            countryService = new CountriesService(dbContext);
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
            var action = (async () =>
            {
                await countryService.AddCountry(countryAddRequest);
                await countryService.AddCountry(countryAddRequest);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task AddCountry_ValidRequest()
        {
            var countryAddRequest = fixture.Create<CountryAddRequest>();
            var countryResponse = await countryService.AddCountry(countryAddRequest);
            var countries = await countryService.GetAllCountries();
            countries.Should().NotBeNull();
            countryResponse.CountryName.Should().Be(countryAddRequest.CountryName);
            countryResponse.CountryID.Should().NotBeEmpty();
            countries.Should().Contain(countryResponse);
        }
        #endregion

        #region GetAllCountries
        //unit test for GetAllCountries
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            var actual = await countryService.GetAllCountries();
            actual.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllCountries_ValidRequest()
        {
            var requests = fixture.CreateMany<CountryAddRequest>(3);

            var expected = await Task.WhenAll(requests.Select(async request => await countryService.AddCountry(request)));
            //List<CountryResponse> expected = new();
            //foreach (var request in requests)
            //{
            //    expected.Add(await countryService.AddCountry(request));
            //}
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
            var countryResponse = await countryService.GetCountry(Guid.NewGuid());
            countryResponse.Should().BeNull();
        }
        [Fact]
        public async Task GetCountry_ValidRequest()
        {
            var request = fixture.Create<CountryAddRequest>();
            var expected = await countryService.AddCountry(request);
            var actual = await countryService.GetCountry(expected.CountryID);
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}
