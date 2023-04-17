using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using EntityFrameworkCoreMock;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;

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
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await countryService.AddCountry(null));
        }
        [Fact]
        public async Task AddCountry_NullCountryName()
        {
            var countryAddRequest = fixture.Build<CountryAddRequest>().With(r => r.CountryName, null as string).Create();
            await Assert.ThrowsAsync<ArgumentException>(async () => await countryService.AddCountry(countryAddRequest));
        }
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            var countryAddRequest = fixture.Create<CountryAddRequest>();
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await countryService.AddCountry(countryAddRequest);
                await countryService.AddCountry(countryAddRequest);
            });
        }
        [Fact]
        public async Task AddCountry_ValidRequest()
        {
            var countryAddRequest = fixture.Create<CountryAddRequest>();
            var countryResponse = await countryService.AddCountry(countryAddRequest);
            var countries = await countryService.GetCountries();
            Assert.NotNull(countryResponse);
            Assert.Equal("United States", countryResponse.CountryName);
            Assert.True(countryResponse.CountryID != Guid.Empty);
            Assert.Contains(countryResponse, countries);
        }
        #endregion

        #region GetCountries
        //unit test for GetCountries
        [Fact]
        public async Task GetCountries_EmptyList()
        {
            var actual = await countryService.GetCountries();
            Assert.Empty(actual);
        }
        [Fact]
        public async Task GetCountries_ValidRequest()
        {
            var requests = fixture.CreateMany<CountryAddRequest>(3);

            var expected = await Task.WhenAll(requests.Select(async request => await countryService.AddCountry(request)));
            //List<CountryResponse> expected = new();
            //foreach (var request in requests)
            //{
            //    expected.Add(await countryService.AddCountry(request));
            //}
            var actual = await countryService.GetCountries();

            Assert.Equal(expected.Length, actual.Count);
            foreach (var country in expected)
            {
                Assert.Contains(country, actual);
            }
        }
        #endregion

        #region GetCountry
        [Fact]
        public async Task GetCountry_NullCountryID()
        {
            var countryResponse = await countryService.GetCountry(null);
            Assert.Null(countryResponse);
        }
        [Fact]
        public async Task GetCountry_NonExistingCountryID()
        {
            var countryResponse = await countryService.GetCountry(Guid.NewGuid());
            Assert.Null(countryResponse);
        }
        [Fact]
        public async Task GetCountry_ValidRequest()
        {
            var request = fixture.Create<CountryAddRequest>();
            var expected = await countryService.AddCountry(request);
            var actual = await countryService.GetCountry(expected.CountryID);
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
