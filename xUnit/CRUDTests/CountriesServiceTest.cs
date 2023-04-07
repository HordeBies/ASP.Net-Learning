using Entities;
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
            countryService = new CountriesService();
        }

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
            Assert.Throws<ArgumentException>(() => countryService.AddCountry(countryAddRequest));
        }
        [Fact]
        public void AddCountry_ValidRequest()
        {
            var countryAddRequest = new CountryAddRequest
            {
                CountryName = "United States"
            };
            var countryResponse = countryService.AddCountry(countryAddRequest);
            Assert.NotNull(countryResponse);
            Assert.Equal("United States", countryResponse.CountryName);
            Assert.True(countryResponse.CountryID != Guid.Empty);
        }
    }
}
