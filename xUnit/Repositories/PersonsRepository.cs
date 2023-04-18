using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext db;
        public PersonsRepository(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }
        public async Task<Person> AddPerson(Person person)
        {
            db.Persons.Add(person);
            await db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePerson(Guid personID)
        {
            db.Persons.RemoveRange(db.Persons.Where(p => p.PersonID.Equals(personID)));
            int rowsDeleted = await db.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await db.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPerson(Guid personID)
        {
            return await db.Persons.FirstOrDefaultAsync(p => p.PersonID.Equals(personID));
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            var match = await db.Persons.FirstOrDefaultAsync(p => p.PersonID.Equals(person.PersonID));
            if (match == null)
                return person;
            match.Address = person.Address;
            match.CountryID = person.CountryID;
            match.DateOfBirth = person.DateOfBirth;
            match.Email = person.Email;
            match.Gender = person.Gender;
            match.PersonName = person.PersonName;
            match.ReceiveNewsLetters = person.ReceiveNewsLetters;

            await db.SaveChangesAsync();
            return match;
        }
    }
}
