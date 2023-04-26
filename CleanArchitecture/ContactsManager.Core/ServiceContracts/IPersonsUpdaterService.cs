using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for Person entity
    /// </summary>
    public interface IPersonsUpdaterService
    {
        /// <summary>
        /// Updates an existing person with the specified changes.
        /// </summary>
        /// <param name="request">The request object that contains the person's updated information.</param>
        /// <returns>A response object that contains the updated person's information.</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? request);

    }
}
