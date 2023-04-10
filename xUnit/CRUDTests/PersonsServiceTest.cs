using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using System.Text;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService personsService;
        private readonly ICountriesService countriesService;
        private readonly ITestOutputHelper testOutputHelper;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            personsService = new PersonsService();
            countriesService = new CountriesService();
            this.testOutputHelper = testOutputHelper;
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

        #region GetPerson
        [Fact]
        public void GetPerson_NullPersonID()
        {
            var expected = personsService.GetPerson(null);
            Assert.Null(expected);
        }
        [Fact]
        public void GetPerson_InvalidPersonID()
        {
            var expected = personsService.GetPerson(Guid.NewGuid());
            Assert.Null(expected);
        }
        [Fact]
        public void GetPerson_ValidPersonID()
        {
            var countryAddRequest = new CountryAddRequest
            {
                CountryName = "United States"
            };
            var countryResponse = countriesService.AddCountry(countryAddRequest);
            var personAddRequest = new PersonAddRequest
            {
                PersonName = "John",
                Email = "test@test.com",
                Address = "sample address",
                DateOfBirth = new(2000,6,12),
                Gender = GenderOptions.Other,
                ReceieveNewsLetters = true,
                CountryID = countryResponse.CountryID,
            };
            var expected = personsService.AddPerson(personAddRequest);
            var actual = personsService.GetPerson(expected.PersonID);
            Assert.Equal(expected, actual);
        }
        #endregion

        #region GetPersons
        [Fact]
        public void GetPersons_EmptyCollection()
        {
            var expected = personsService.GetPersons();
            Assert.Empty(expected);
        }
        [Fact]
        public void GetPersons_ValidCollection()
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
            var countries = countryAddRequests.Select(c => countriesService.AddCountry(c)).ToList();
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

            var expected = personAddRequests.Select(p => personsService.AddPerson(p)).ToList();
            var actual = personsService.GetPersons();
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
            Assert.Equal(expected.Count, actual.Count);
            foreach (var person in expected)
            {
                Assert.Contains(person, actual);
            }
        }
        #endregion

        #region GetFilteredPersons
        //If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
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
            var countries = countryAddRequests.Select(c => countriesService.AddCountry(c)).ToList();
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

            var expected = personAddRequests.Select(p => personsService.AddPerson(p)).ToList();
            var actual = personsService.GetFilteredPersons(nameof(PersonResponse.PersonName),"");
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
            Assert.Equal(expected.Count, actual.Count);
            foreach (var person in expected)
            {
                Assert.Contains(person, actual);
            }
        }
        [Fact]
        public void GetFilteredPersons_ValidSearch()
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
            var countries = countryAddRequests.Select(c => countriesService.AddCountry(c)).ToList();
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

            var expected = personAddRequests.Select(p => personsService.AddPerson(p)).ToList();
            var actual = personsService.GetFilteredPersons(nameof(PersonResponse.PersonName),"ha");
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
        public void GetSortedPersons_ValidSort()
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
            var countries = countryAddRequests.Select(c => countriesService.AddCountry(c)).ToList();
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

            var expected = personAddRequests.Select(p => personsService.AddPerson(p)).ToList();
            var actual = personsService.GetSortedPersons(personsService.GetPersons(),nameof(PersonResponse.PersonName),SortOrder.Descending);
            expected = expected.OrderByDescending(i => i.PersonName).ToList();
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
    }
}