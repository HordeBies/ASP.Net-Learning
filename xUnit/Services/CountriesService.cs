using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
        {
            this.countriesRepository = countriesRepository;
        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Request is null");
            }
            if (string.IsNullOrEmpty(request.CountryName))
            {
                throw new ArgumentException("Country name is null or empty");
            }
            if(await countriesRepository.GetCountry(request.CountryName) != null)
            {
                throw new ArgumentException("Country name already exists");
            }
            var country = request.ToCountry(); 
            country.CountryID = Guid.NewGuid();
            await countriesRepository.AddCountry(country);
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            
            return (await countriesRepository.GetAllCountries()).Select(c=> c.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountry(Guid? countryID)
        {
            if (countryID == null)
                return null;
            var country = await countriesRepository.GetCountry(countryID.Value);
            return country?.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream stream = new();
            await formFile.CopyToAsync(stream);
            int insertedCount = 0;
            using(ExcelPackage excelPackage = new(stream))
            {
                var sheet = excelPackage.Workbook.Worksheets["Countries"];
                int rowCount = sheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(sheet.Cells[row, 1].Value);
                    try
                    {
                        await AddCountry(new() { CountryName = cellValue });
                        insertedCount++;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return insertedCount;
        }
    }
}