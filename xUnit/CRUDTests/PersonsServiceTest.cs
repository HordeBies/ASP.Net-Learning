using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService personsService;
        public PersonsServiceTest()
        {
            personsService = new PersonsService();
        }
        #region AddPerson
        [Fact]
        public void AddPerson_NullRequest()
        {
            Assert.Throws<ArgumentNullException>(() => personsService.AddPerson(null));
        }
        [Fact]
        public void AddPerson_NullPersonName()
        {
            var personAddRequest = new PersonAddRequest
            {
                PersonName = null
            };
            Assert.Throws<ArgumentException>(() => personsService.AddPerson(personAddRequest));
        }
        [Fact]
        public void AddPerson_ValidRequest()
        {
            var personAddRequest = new PersonAddRequest
            {
                PersonName = "John",
                Address = "sample address",
                Email = "john@bies.com",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = new(2000,1,1),
                ReceieveNewsLetters = true
            };
            var expected = personsService.AddPerson(personAddRequest);
            var collection = personsService.GetPersons();
            Assert.True(expected.PersonID != Guid.Empty);
            Assert.Contains(expected, collection);
        }
        #endregion
    }
}