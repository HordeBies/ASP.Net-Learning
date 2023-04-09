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
    }
}
