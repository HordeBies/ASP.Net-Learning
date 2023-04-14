using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public PersonsDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed
            var c = File.ReadAllText("countries_seed.json");
            var countries = JsonSerializer.Deserialize<List<Country>>(c);
            foreach (var country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            var p = File.ReadAllText("persons_seed.json");
            var persons = JsonSerializer.Deserialize<List<Person>>(p);
            foreach (var person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
        }
    }
}
