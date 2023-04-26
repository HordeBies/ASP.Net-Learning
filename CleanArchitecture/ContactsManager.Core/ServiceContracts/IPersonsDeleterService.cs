
namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for Person entity
    /// </summary>
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Deletes a person with the specified ID.
        /// </summary>
        /// <param name="PersonID">The ID of the person to delete.</param>
        /// <returns>True if the person was successfully deleted; otherwise, false.</returns>
        Task<bool> DeletePerson(Guid? PersonID);

    }
}
