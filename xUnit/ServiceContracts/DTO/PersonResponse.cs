﻿using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of PersonService methods
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;
            var other = (PersonResponse)obj;
            return PersonID == other.PersonID &&
                PersonName == other.PersonName &&
                Email == other.Email &&
                DateOfBirth == other.DateOfBirth &&
                Gender == other.Gender &&
                CountryID == other.CountryID &&
                Country == other.Country &&
                Address == other.Address &&
                ReceiveNewsLetters == other.ReceiveNewsLetters &&
                Age == other.Age;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class PersonResponseExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = person.DateOfBirth == null ? null : Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays/365.25)
            };
        }
    }
}