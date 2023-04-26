using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Serilog;

namespace ContactsManager.Core.Services
{
    public class PersonsGetterServiceChild_CompactExcel : PersonsGetterService
    {
        public PersonsGetterServiceChild_CompactExcel(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext) : base(personsRepository, logger, diagnosticContext)
        {
        }
        public override async Task<MemoryStream> GetPersonsExcel() //this is somewhat violates LSP, but it's ok for now to demonstrate OCP
        {
            MemoryStream stream = new();
            using (ExcelPackage excelPackage = new(stream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                workSheet.Cells["A1"].Value = nameof(PersonResponse.PersonName);
                workSheet.Cells["B1"].Value = nameof(PersonResponse.Age);
                workSheet.Cells["C1"].Value = nameof(PersonResponse.Gender);


                using (ExcelRange headerCells = workSheet.Cells["A1:C1"])
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
                    workSheet.Cells[row, 2].Value = person.Age;
                    workSheet.Cells[row, 3].Value = person.Gender;
                    row++;
                }

                workSheet.Cells[$"A1:C{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }
            stream.Position = 0;
            return stream;
        }
    }
}
