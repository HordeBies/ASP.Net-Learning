using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using System.Text;
using Xunit.Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService personsService;
        private readonly ICountriesService countriesService;
        private readonly ITestOutputHelper testOutputHelper;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
            personsService = new PersonsService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
            this.testOutputHelper = testOutputHelper;
        }
        #region AddPerson
        [Fact]
        public async Task AddPerson_NullRequest()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await personsService.AddPerson(null));
        }
        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            var personAddRequest = new PersonAddRequest
            {
                PersonName = null
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => await personsService.AddPerson(personAddRequest));
        }
        [Fact]
        public async Task AddPerson_ValidRequest()
        {
            var personAddRequest = new PersonAddRequest
            {
                PersonName = "John",
                Address = "sample address",
                Email = "john@bies.com",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = new(2000,1,1),
                ReceiveNewsLetters = true
            };
            var expected = await personsService.AddPerson(personAddRequest);
            var collection = await personsService.GetPersons();
            Assert.True(expected.PersonID != Guid.Empty);
            Assert.Contains(expected, collection);
        }
        #endregion

        #region GetPerson
        [Fact]
        public async Task GetPerson_NullPersonID()
        {
            var expected = await personsService.GetPerson(null);
            Assert.Null(expected);
        }
        [Fact]
        public async Task GetPerson_InvalidPersonID()
        {
            var expected = await personsService.GetPerson(Guid.NewGuid());
            Assert.Null(expected);
        }
        [Fact]
        public async Task GetPerson_ValidPersonID()
        {
            var countryAddRequest = new CountryAddRequest
            {
                CountryName = "United States"
            };
            var countryResponse = await countriesService.AddCountry(countryAddRequest);
            var personAddRequest = new PersonAddRequest
            {
                PersonName = "John",
                Email = "test@test.com",
                Address = "sample address",
                DateOfBirth = new(2000,6,12),
                Gender = GenderOptions.Other,
                ReceiveNewsLetters = true,
                CountryID = countryResponse.CountryID,
            };
            var expected = await personsService.AddPerson(personAddRequest);
            var actual = await personsService.GetPerson(expected.PersonID);
            Assert.Equal(expected, actual);
        }
        #endregion

        #region GetPersons
        [Fact]
        public async Task GetPersons_EmptyCollection()
        {
            var expected = await personsService.GetPersons();
            Assert.Empty(expected);
        }
        [Fact]
        public async Task GetPersons_ValidCollection()
        {
            var countryAddRequests = new List<CountryAddRequest>
            {
                new()
                {
                    CountryName = "United States"
                },
                new()
                {
                    CountryName = "India"
                }
            };
            var countries = await Task.WhenAll(countryAddRequests.Select(async request =>await countriesService.AddCountry(request)));
            var personAddRequests = new List<PersonAddRequest>
            {
                new()
                {
                    PersonName = "person1",
                    Email = "mail1@google.com",
                    CountryID = countries[0].CountryID,
                },
                new()
                {
                    PersonName ="person2",
                    Email = "mail2@google.com",
                    CountryID = countries[1].CountryID,
                },
                new()
                {
                    PersonName ="person3",
                    Email = "mail3@google.com",
                    CountryID = countries[0].CountryID
                }
            };

            var expected = await Task.WhenAll(personAddRequests.Select(async request => await personsService.AddPerson(request)));
            var actual = await personsService.GetPersons();
            testOutputHelper.WriteLine("Expected:");
            foreach (var item in expected)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            testOutputHelper.WriteLine("Actual:");
            foreach (var item in actual)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            Assert.Equal(expected.Length, actual.Count);
            foreach (var person in expected)
            {
                Assert.Contains(person, actual);
            }
        }
        #endregion

        #region GetFilteredPersons
        //If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            var countryAddRequests = new List<CountryAddRequest>
            {
                new()
                {
                    CountryName = "United States"
                },
                new()
                {
                    CountryName = "India"
                }
            };
            var countries = await Task.WhenAll(countryAddRequests.Select(async request => await countriesService.AddCountry(request)));
            var personAddRequests = new List<PersonAddRequest>
            {
                new()
                {
                    PersonName = "person1",
                    Email = "mail1@google.com",
                    CountryID = countries[0].CountryID,
                },
                new()
                {
                    PersonName ="person2",
                    Email = "mail2@google.com",
                    CountryID = countries[1].CountryID,
                },
                new()
                {
                    PersonName ="person3",
                    Email = "mail3@google.com",
                    CountryID = countries[0].CountryID
                }
            };

            var expected = await Task.WhenAll(personAddRequests.Select(async request => await personsService.AddPerson(request)));
            var actual = await personsService.GetFilteredPersons(nameof(PersonResponse.PersonName),"");
            testOutputHelper.WriteLine("Expected:");
            foreach (var item in expected)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            testOutputHelper.WriteLine("Actual:");
            foreach (var item in actual)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            Assert.Equal(expected.Length, actual.Count);
            foreach (var person in expected)
            {
                Assert.Contains(person, actual);
            }
        }
        [Fact]
        public async Task GetFilteredPersons_ValidSearch()
        {
            var countryAddRequests = new List<CountryAddRequest>
            {
                new()
                {
                    CountryName = "United States"
                },
                new()
                {
                    CountryName = "India"
                }
            };
            var countries = await Task.WhenAll(countryAddRequests.Select(async request => await countriesService.AddCountry(request)));
            var personAddRequests = new List<PersonAddRequest>
            {
                new()
                {
                    PersonName = "Hasan",
                    Email = "mail1@google.com",
                    CountryID = countries[0].CountryID,
                },
                new()
                {
                    PersonName ="Mehmet",
                    Email = "mail2@google.com",
                    CountryID = countries[1].CountryID,
                },
                new()
                {
                    PersonName ="Hayri",
                    Email = "mail3@google.com",
                    CountryID = countries[0].CountryID
                }
            };

            var expected = await Task.WhenAll(personAddRequests.Select(async request => await personsService.AddPerson(request)));
            var actual = await personsService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ha");
            testOutputHelper.WriteLine("Expected:");
            foreach (var item in expected)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            testOutputHelper.WriteLine("Actual:");
            foreach (var item in actual)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            foreach (var person in expected)
            {
                if(person.PersonName != null && person.PersonName.Contains("ha",StringComparison.OrdinalIgnoreCase))
                    Assert.Contains(person, actual);
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons_ValidSort()
        {
            var countryAddRequests = new List<CountryAddRequest>
            {
                new()
                {
                    CountryName = "United States"
                },
                new()
                {
                    CountryName = "India"
                }
            };
            var countries = await Task.WhenAll(countryAddRequests.Select(async request => await countriesService.AddCountry(request)));
            var personAddRequests = new List<PersonAddRequest>
            {
                new()
                {
                    PersonName = "Hasan",
                    Email = "mail1@google.com",
                    CountryID = countries[0].CountryID,
                },
                new()
                {
                    PersonName ="Mehmet",
                    Email = "mail2@google.com",
                    CountryID = countries[1].CountryID,
                },
                new()
                {
                    PersonName ="Hayri",
                    Email = "mail3@google.com",
                    CountryID = countries[0].CountryID
                }
            };

            var query = await Task.WhenAll(personAddRequests.Select(async request => await personsService.AddPerson(request)));
            var actual = await personsService.GetSortedPersons(await personsService.GetPersons(), nameof(PersonResponse.PersonName), SortOrder.Descending);
            var expected = new List<PersonResponse>(query).OrderByDescending(i => i.PersonName).ToList();
            testOutputHelper.WriteLine("Expected:");
            foreach (var item in expected)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            testOutputHelper.WriteLine("Actual:");
            foreach (var item in actual)
            {
                testOutputHelper.WriteLine(item.ToString());
            }
            for(int idx = 0; idx < expected.Count; idx++)
            {
                Assert.Equal(expected[idx], actual[idx]);
            }
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_NullRequest()
        {
            PersonUpdateRequest? request = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async() => await personsService.UpdatePerson(request));
        }
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            var request = new PersonUpdateRequest
            {
                PersonID = Guid.NewGuid(), // Invalid person ID
                PersonName = "John Smith",
                Email = "john.smith@example.com",
                Address = "123 Main St",
                DateOfBirth = new DateTime(1980, 1, 1),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            await Assert.ThrowsAsync<ArgumentException>(async() => await personsService.UpdatePerson(request));
        }
        [Fact]
        public async Task UpdatePerson_NullPersonName()
        {
            var person = await personsService.AddPerson(new PersonAddRequest
            {
                PersonName = "John Smith",
                Email = "john.smith@example.com",
                Address = "123 Main St",
                DateOfBirth = new DateTime(1980, 1, 1),
                Gender = GenderOptions.Male,
            });
            var request = person.ToPersonUpdateRequest();
            request.PersonName = null;

            await Assert.ThrowsAsync<ArgumentException>(async () => await personsService.UpdatePerson(request));
        }
        [Fact]
        public async Task UpdatePerson_ValidPersonID()
        {
            var person = await personsService.AddPerson(new PersonAddRequest
            {
                PersonName = "John Smith",
                Email = "john.smith@example.com",
                Address = "123 Main St",
                DateOfBirth = new DateTime(1980, 1, 1),
                Gender = GenderOptions.Male,
            });

            var request = person.ToPersonUpdateRequest();
            request.PersonName = "Mehmet Demirci";
            request.Email = "oa.mehmetdmrc@gmail.com";

            var expected = await personsService.UpdatePerson(request);
            var actual = await personsService.GetPerson(request.PersonID);

            Assert.Equal(expected,actual);
        }
        #endregion

        #region DeletePerson
        [Fact]
        public async Task DeletePerson_NonExistentID()
        {
            Guid nonExistentID = Guid.NewGuid();

            bool result = await personsService.DeletePerson(nonExistentID);

            Assert.False(result);
        }

        [Fact]
        public async Task DeletePerson_ValidID_ReturnsTrue()
        {
            var personAddRequest = new PersonAddRequest
            {
                PersonName = "John",
                Email = "test@test.com",
                Address = "sample address",
                DateOfBirth = new DateTime(2000, 6, 12),
                Gender = GenderOptions.Other,
                ReceiveNewsLetters = true,
                CountryID = null,
            };
            var addedPerson = await personsService.AddPerson(personAddRequest);

            bool result = await personsService.DeletePerson(addedPerson.PersonID);

            var expected = await personsService.GetPerson(addedPerson.PersonID);

            Assert.True(result);
            Assert.Null(expected);
        }
        #endregion
    }
}