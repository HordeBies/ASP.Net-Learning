using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Person entity.
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a new person to the repository.
        /// </summary>
        /// <param name="person">The person object to add to the repository.</param>
        /// <returns>The newly added person object.</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Retrieves a list of all persons in the repository.
        /// </summary>
        /// <returns>A list of all persons in the repository.</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Retrieves a specific person object from the repository based on the provided ID.
        /// </summary>
        /// <param name="personID">The ID of the person to retrieve.</param>
        /// <returns>The person object with the specified ID or null if it does not exist.</returns>
        Task<Person?> GetPerson(Guid personID);

        /// <summary>
        /// Retrieves a list of person objects from the repository that match the specified search criteria.
        /// </summary>
        /// <param name="predicate">The filter criteria to apply to the person collection.</param>
        /// <returns>A list of person objects that match the search criteria.</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person,bool>> predicate);

        /// <summary>
        /// Updates an existing person object in the repository with the specified changes.
        /// </summary>
        /// <param name="person">The person object to update.</param>
        /// <returns>The updated person object.</returns>
        Task<Person> UpdatePerson(Person person);

        /// <summary>
        /// Deletes the person with the specified ID from the repository.
        /// </summary>
        /// <param name="personID">The ID of the person to delete.</param>
        /// <returns>The boolean value that indicates whether the operation was successful.</returns>
        Task<bool> DeletePerson(Guid personID);



    }
}
