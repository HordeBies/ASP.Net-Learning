using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for Person entity
    /// </summary>
    public interface IPersonsGetterService
    {
        /// <summary>
        /// Retrieves a list of all people.
        /// </summary>
        /// <returns>A list of person objects.</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Retrieves a person object with the specified ID.
        /// </summary>
        /// <param name="PersonID">The ID of the person to retrieve.</param>
        /// <returns>The person object with the specified ID, or null if not found.</returns>
        Task<PersonResponse?> GetPerson(Guid? PersonID);

        /// <summary>
        /// Retrieves a list of person objects that match the specified search criteria.
        /// </summary>
        /// <param name="searchby">The field to search in.</param>
        /// <param name="searchString">The string to search for.</param>
        /// <returns>A list of person objects that match the search criteria.</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchby, string? searchString);

        /// <summary>
        /// Retrieves all people as a CSV file.
        /// </summary>
        /// <returns>A MemoryStream containing the CSV file data.</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Retrieves all people as an Excel file.
        /// </summary>
        /// <returns>A MemoryStream containing the Excel file data.</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
