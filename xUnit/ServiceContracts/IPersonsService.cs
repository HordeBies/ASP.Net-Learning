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
        /// Adds a person object to the list of people
        /// </summary>
        /// <param name="request">Person object to add</param>
        /// <returns>Returns the person object after adding it</returns>
        PersonResponse AddPerson(PersonAddRequest? request);

        /// <summary>
        /// Returns all people
        /// </summary>
        /// <returns>A list of <see cref="PersonResponse"/> objects</returns>
        List<PersonResponse> GetPersons();

        /// <summary>
        /// Returns a person object based on the PersonID
        /// </summary>
        /// <param name="PersonID">PersonID (guid) to search</param>
        /// <returns>Matching country as <see cref="PersonResponse"/> object</returns>
        PersonResponse? GetPerson(Guid? PersonID);

        /// <summary>
        /// Returns all person objects that match the search criteria
        /// </summary>
        /// <param name="searchby">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>All matching persons based on the given search field and search string</returns>
        public List<PersonResponse> GetFilteredPersons(string searchby, string? searchString);

        /// <summary>
        /// Sorts the given collection of PersonResponse objects by the specified property and sort order.
        /// </summary>
        /// <param name="collection">The collection of PersonResponse objects to sort.</param>
        /// <param name="sortby">The name of the property to sort by.</param>
        /// <param name="sortOrder">The sort order to use (ascending or descending).</param>
        /// <returns>A sorted List of PersonResponse objects.</returns>
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> collectiong, string sortby, Enums.SortOrder sortOrder);
    }
}
