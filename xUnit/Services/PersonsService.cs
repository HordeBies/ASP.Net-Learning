﻿using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private PersonsDbContext db;
        public PersonsService(PersonsDbContext personsDbContext)
        {
            db = personsDbContext;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            ValidationHelper.ModelValidation(request);

            var person = request.ToPerson();
            person.PersonID = Guid.NewGuid();
            db.Persons.Add(person);
            await db.SaveChangesAsync();
            //db.sp_InsertPerson(person);

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetPersons()
        {
            var persons = await db.Persons.Include(nameof(Person.Country)).ToListAsync();
            return persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPerson(Guid? PersonID)
        {
            if (PersonID == null) return null;
            var person = await db.Persons.Include(nameof(Person.Country)).FirstOrDefaultAsync(p => p.PersonID == PersonID);
            if (person == null) return null;
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchby, string? searchString)
        {
            var allPersons = await GetPersons();
            List<PersonResponse> matchingPersons = new();
            if (string.IsNullOrEmpty(searchby) || string.IsNullOrEmpty(searchString))
                return allPersons;
            switch (searchby)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(p => p.PersonName != null && p.PersonName.Contains(searchString,StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(p => p.Address != null && p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
                case nameof(PersonResponse.Country):
                    matchingPersons = allPersons.Where(p => p.Country != null && p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(p => p.Email != null && p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Age):
                    matchingPersons = allPersons.Where(p => p.Age != null && p.Age.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(p => p.DateOfBirth != null && p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(p => p.Gender != null && p.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.ReceiveNewsLetters):
                    matchingPersons = allPersons.Where(p => p.ReceiveNewsLetters.ToString()!.Equals(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    matchingPersons = allPersons;
                    break;
            }
            return matchingPersons;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> collection, string sortby, SortOrder sortOrder)
        {
            if (string.IsNullOrEmpty(sortby))
                return collection;
            List<PersonResponse> sorted = sortby switch
            {
                nameof(PersonResponse.PersonName) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.PersonName).ToList() : collection.OrderByDescending(i => i.PersonName).ToList(),
                nameof(PersonResponse.Address) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Address).ToList() : collection.OrderByDescending(i => i.Address).ToList(),
                nameof(PersonResponse.Country) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Country).ToList() : collection.OrderByDescending(i => i.Country).ToList(),
                nameof(PersonResponse.Email) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Email).ToList() : collection.OrderByDescending(i => i.Email).ToList(),
                nameof(PersonResponse.Age) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Age).ToList() : collection.OrderByDescending(i => i.Age).ToList(),
                nameof(PersonResponse.DateOfBirth) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.DateOfBirth).ToList() : collection.OrderByDescending(i => i.DateOfBirth).ToList(),
                nameof(PersonResponse.Gender) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.Gender).ToList() : collection.OrderByDescending(i => i.Gender).ToList(),
                nameof(PersonResponse.ReceiveNewsLetters) => sortOrder == SortOrder.Ascending ? collection.OrderBy(i => i.ReceiveNewsLetters).ToList() : collection.OrderByDescending(i => i.ReceiveNewsLetters).ToList(),
                _ => collection
            };
            return sorted;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ValidationHelper.ModelValidation(request);
            var match = await db.Persons.FirstOrDefaultAsync(p => p.PersonID == request.PersonID);
            if (match == null) throw new ArgumentException("Given person does not exist");
            match.PersonName = request.PersonName;
            match.Address = request.Address;
            match.CountryID = request.CountryID;
            match.DateOfBirth = request.DateOfBirth;
            match.Email = request.Email;
            match.Gender = request.Gender.ToString();
            match.ReceiveNewsLetters = request.ReceiveNewsLetters;
            await db.SaveChangesAsync();
            return match.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? PersonID)
        {
            if (PersonID == null || PersonID == Guid.Empty) return false;
            var person = await db.Persons.FirstOrDefaultAsync(p => p.PersonID == PersonID);
            if (person == null) return false;
            db.Remove(person);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream stream = new();
            StreamWriter writer = new StreamWriter(stream);
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new(writer,csvConfiguration,true);
            //csvWriter.WriteHeader<PersonResponse>();
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
            csvWriter.NextRecord();
            var persons = await GetPersons();
            foreach (var person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                csvWriter.WriteField(person.Age);
                csvWriter.WriteField(person.DateOfBirth?.ToString("yyyy-MM-dd"));
                csvWriter.WriteField(person.Gender);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.ReceiveNewsLetters);
                csvWriter.NextRecord();
                csvWriter.Flush();
            }
            //await csvWriter.WriteRecordsAsync(persons);
            stream.Position = 0;
            return stream;
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream stream = new();
            using(ExcelPackage excelPackage = new(stream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                workSheet.Cells["A1"].Value = nameof(PersonResponse.PersonName);
                workSheet.Cells["B1"].Value = nameof(PersonResponse.Email);
                workSheet.Cells["C1"].Value = nameof(PersonResponse.DateOfBirth);
                workSheet.Cells["D1"].Value = nameof(PersonResponse.Age);
                workSheet.Cells["E1"].Value = nameof(PersonResponse.Gender);
                workSheet.Cells["F1"].Value = nameof(PersonResponse.Country);
                workSheet.Cells["G1"].Value = nameof(PersonResponse.Address);
                workSheet.Cells["H1"].Value = nameof(PersonResponse.ReceiveNewsLetters);

                using(ExcelRange headerCells = workSheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                var persons = await GetPersons();
                foreach (var person in persons)
                {
                    workSheet.Cells[row,1].Value = person.PersonName;
                    workSheet.Cells[row,2].Value = person.Email;
                    workSheet.Cells[row,3].Value = person.DateOfBirth;
                    workSheet.Cells[row,4].Value = person.Age;
                    workSheet.Cells[row,5].Value = person.Gender;
                    workSheet.Cells[row,6].Value = person.Country;
                    workSheet.Cells[row,7].Value = person.Address;
                    workSheet.Cells[row,8].Value = person.ReceiveNewsLetters;
                    row++;
                }

                workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }
            stream.Position = 0;
            return stream;
        }
    }
}
