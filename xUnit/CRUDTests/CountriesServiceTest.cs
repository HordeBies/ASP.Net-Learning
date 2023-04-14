using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService countryService;
        public CountriesServiceTest()
        {
            countryService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }
        #region AddCountry
        [Fact]
        public void AddCountry_NullRequest()
        {
            Assert.Throws<ArgumentNullException>(() => countryService.AddCountry(null));
        }
        [Fact]
        public void AddCountry_NullCountryName()
        {
            var countryAddRequest = new CountryAddRequest
            {
                CountryName = null
            };
            Assert.Throws<ArgumentException>(() => countryService.AddCountry(countryAddRequest));
        }
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            var countryAddRequest = new CountryAddRequest
            {
                CountryName = "India"
            };
            Assert.Throws<ArgumentException>(() =>
            {
                countryService.AddCountry(countryAddRequest);
                countryService.AddCountry(countryAddRequest);
            });
        }
        [Fact]
        public void AddCountry_ValidRequest()
        {
            var countryAddRequest = new CountryAddRequest
            {
                CountryName = "United States"
            };
            var countryResponse = countryService.AddCountry(countryAddRequest);
            var countries = countryService.GetCountries();
            Assert.NotNull(countryResponse);
            Assert.Equal("United States", countryResponse.CountryName);
            Assert.True(countryResponse.CountryID != Guid.Empty);
            Assert.Contains(countryResponse, countries);
        }
        #endregion

        #region GetCountries
        //unit test for GetCountries
        [Fact]
        public void GetCountries_EmptyList()
        {
            var actual = countryService.GetCountries();
            Assert.Empty(actual);
        }
        [Fact]
        public void GetCountries_ValidRequest()
        {
            var requests = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "Romania" },
                new CountryAddRequest() { CountryName = "France" },
                new CountryAddRequest () { CountryName = "Germany" }
            };

            var expected = requests.Select(request => countryService.AddCountry(request)).ToList();

            var actual = countryService.GetCountries();

            Assert.Equal(expected.Count, actual.Count);
            foreach (var country in expected)
            {
                Assert.Contains(country, actual);
            }
        }
        #endregion

        #region GetCountry
        [Fact]
        public void GetCountry_NullCountryID()
        {
            var countryResponse = countryService.GetCountry(null);
            Assert.Null(countryResponse);
        }
        [Fact]
        public void GetCountry_NonExistingCountryID()
        {
            var countryResponse = countryService.GetCountry(Guid.NewGuid());
            Assert.Null(countryResponse);
        }
        [Fact]
        public void GetCountry_ValidRequest()
        {
            var request = new CountryAddRequest()
            {
                CountryName = "Romania"
            };
            var expected = countryService.AddCountry(request);
            var actual = countryService.GetCountry(expected.CountryID);
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
