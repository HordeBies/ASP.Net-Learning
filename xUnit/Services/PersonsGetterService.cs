﻿using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;

namespace Services
{
    public class PersonsGetterService : IPersonsGetterService
    {
        private readonly IPersonsRepository personsRepository;
        private readonly ILogger<PersonsGetterService> logger;
        private readonly IDiagnosticContext diagnosticContext;
        public PersonsGetterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            this.personsRepository = personsRepository;
            this.logger = logger;
            this.diagnosticContext = diagnosticContext;
        }

        public virtual async Task<List<PersonResponse>> GetAllPersons()
        {
            logger.LogInformation("GetAllPersons method is called");
            return (await personsRepository.GetAllPersons()).Select(p => p.ToPersonResponse()).ToList();
        }

        public virtual async Task<PersonResponse?> GetPerson(Guid? PersonID)
        {
            logger.LogInformation("GetPerson method is called");
            if (PersonID == null) return null;
            var person = await personsRepository.GetPerson(PersonID.Value);
            if (person == null) return null;
            return person.ToPersonResponse();
        }

        public virtual async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            logger.LogInformation("GetFilteredPersons method is called");
            List<Person> persons;
            if (string.IsNullOrEmpty(searchString))
            {
                persons = await personsRepository.GetAllPersons();
            }
            else
            {
                using (Operation.Time("Time for Filtered Persons from Database"))
                {
                    persons = searchBy switch
                    {
                        nameof(PersonResponse.PersonName) =>
                         await personsRepository.GetFilteredPersons(temp =>
                         temp.PersonName.Contains(searchString)),

                        nameof(PersonResponse.Email) =>
                         await personsRepository.GetFilteredPersons(temp =>
                         temp.Email.Contains(searchString)),

                        nameof(PersonResponse.DateOfBirth) =>
                         await personsRepository.GetFilteredPersons(temp =>
                         temp.DateOfBirth.Value.ToString().Contains(searchString)),


                        nameof(PersonResponse.Gender) =>
                         await personsRepository.GetFilteredPersons(temp =>
                         temp.Gender.Equals(searchString)),

                        nameof(PersonResponse.CountryID) =>
                         await personsRepository.GetFilteredPersons(temp =>
                         temp.Country.CountryName.Contains(searchString)),

                        nameof(PersonResponse.Address) =>
                        await personsRepository.GetFilteredPersons(temp =>
                        temp.Address.Contains(searchString)),

                        _ => await personsRepository.GetAllPersons()
                    };
                }
            }
            diagnosticContext.Set("Persons", persons);
            return persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream stream = new();
            StreamWriter writer = new StreamWriter(stream);
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new(writer, csvConfiguration, true);
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
            var persons = await GetAllPersons();
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

        public virtual async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream stream = new();
            using (ExcelPackage excelPackage = new(stream))
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

                using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                var persons = await GetAllPersons();
                foreach (var person in persons)
                {
                    workSheet.Cells[row, 1].Value = person.PersonName;
                    workSheet.Cells[row, 2].Value = person.Email;
                    workSheet.Cells[row, 3].Value = person.DateOfBirth;
                    workSheet.Cells[row, 4].Value = person.Age;
                    workSheet.Cells[row, 5].Value = person.Gender;
                    workSheet.Cells[row, 6].Value = person.Country;
                    workSheet.Cells[row, 7].Value = person.Address;
                    workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;
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
