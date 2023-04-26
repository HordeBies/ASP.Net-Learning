using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for Person entity
    /// </summary>
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a person object to the list of people.
        /// </summary>
        /// <param name="request">The person object to add.</param>
        /// <returns>The person object that was added.</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? request);
    }
}
