using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a person object to the list of people.
        /// </summary>
        /// <param name="request">The person object to add.</param>
        /// <returns>The person object that was added.</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? request);

        /// <summary>
        /// Retrieves a list of all people.
        /// </summary>
        /// <returns>A list of person objects.</returns>
        Task<List<PersonResponse>> GetPersons();

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
        /// Sorts a collection of person objects by the specified property and sort order.
        /// </summary>
        /// <param name="collection">The collection of person objects to sort.</param>
        /// <param name="sortby">The name of the property to sort by.</param>
        /// <param name="sortOrder">The sort order to use (ascending or descending).</param>
        /// <returns>A sorted list of person objects.</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> collectiong, string sortby, Enums.SortOrder sortOrder);

        /// <summary>
        /// Updates an existing person with the specified changes.
        /// </summary>
        /// <param name="request">The request object that contains the person's updated information.</param>
        /// <returns>A response object that contains the updated person's information.</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? request);

        /// <summary>
        /// Deletes a person with the specified ID.
        /// </summary>
        /// <param name="PersonID">The ID of the person to delete.</param>
        /// <returns>True if the person was successfully deleted; otherwise, false.</returns>
        Task<bool> DeletePerson(Guid? PersonID);

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
