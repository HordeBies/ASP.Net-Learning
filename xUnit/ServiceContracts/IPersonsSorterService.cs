using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for Person entity
    /// </summary>
    public interface IPersonsSorterService
    {
        /// <summary>
        /// Sorts a collection of person objects by the specified property and sort order.
        /// </summary>
        /// <param name="collection">The collection of person objects to sort.</param>
        /// <param name="sortby">The name of the property to sort by.</param>
        /// <param name="sortOrder">The sort order to use (ascending or descending).</param>
        /// <returns>A sorted list of person objects.</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> collectiong, string sortby, Enums.SortOrder sortOrder);

    }
}
