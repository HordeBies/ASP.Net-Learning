using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class PersonsSorterService : IPersonsSorterService
    {
        private readonly ILogger<PersonsSorterService> logger;
        public PersonsSorterService(ILogger<PersonsSorterService> logger)
        {
            this.logger = logger;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> collection, string sortby, SortOrder sortOrder)
        {
            logger.LogInformation("GetSortedPersons method is called");
            if (string.IsNullOrEmpty(sortby))
                return collection;
            List<PersonResponse> sorted = sortby switch
            {
                nameof(PersonResponse.PersonName) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.PersonName).ToList() : collection.OrderByDescending(i => i.PersonName).ToList(),
                nameof(PersonResponse.Address) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Address).ToList() : collection.OrderByDescending(i => i.Address).ToList(),
                nameof(PersonResponse.Country) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Country).ToList() : collection.OrderByDescending(i => i.Country).ToList(),
                nameof(PersonResponse.Email) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Email).ToList() : collection.OrderByDescending(i => i.Email).ToList(),
                nameof(PersonResponse.Age) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Age).ToList() : collection.OrderByDescending(i => i.Age).ToList(),
                nameof(PersonResponse.DateOfBirth) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.DateOfBirth).ToList() : collection.OrderByDescending(i => i.DateOfBirth).ToList(),
                nameof(PersonResponse.Gender) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Gender).ToList() : collection.OrderByDescending(i => i.Gender).ToList(),
                nameof(PersonResponse.ReceiveNewsLetters) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.ReceiveNewsLetters).ToList() : collection.OrderByDescending(i => i.ReceiveNewsLetters).ToList(),
                _ => collection
            };
            return sorted;
        }
    }
}
