// <copyright file="PersonsGetterServiceChild.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using OfficeOpenXml;
    using RepositoryContracts;
    using Serilog;

    /// <summary>
    /// Generates an Excel workbook containing person details.
    /// </summary>
    public class PersonsGetterServiceChild : PersonsGetterService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsGetterServiceChild"/> class.
        /// </summary>
        /// <param name="personsRepository">The repository used to access persons.</param>
        /// <param name="logger">The logger used to record service activity.</param>
        /// <param name="diagnosticContext">The context used to enrich diagnostic events.</param>
        public PersonsGetterServiceChild(IPersonsRepository personsRepository, Microsoft.Extensions.Logging.ILogger<PersonsService> logger, IDiagnosticContext diagnosticContext)
            : base(personsRepository, logger, diagnosticContext)
        {
        }

        /// <inheritdoc/>
        public override async Task<MemoryStream> GetPersonsExcel()
        {
            var memoryStream = new MemoryStream();
            using var excelPackage = new ExcelPackage(memoryStream);
            var workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
            workSheet.Cells["A1"].Value = "Person Name";
            workSheet.Cells["B1"].Value = "Age";
            workSheet.Cells["C1"].Value = "Gender";

            using var headerCells = workSheet.Cells["A1:C1"];
            headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            headerCells.Style.Font.Bold = true;

            int row = 2;
            var persons = await this.GetAllPersons();

            // Demo LSP (Adding stricter validation than parent class)
            if (persons.Count == 0)
            {
                throw new InvalidOperationException();
            }

            foreach (var person in persons)
            {
                workSheet.Cells[$"A{row}"].Value = person.PersonName;
                workSheet.Cells[$"B{row}"].Value = person.Age;
                workSheet.Cells[$"C{row}"].Value = person.Gender;
                row++;
            }

            workSheet.Cells[$"A1:C{row}"].AutoFitColumns();
            await excelPackage.SaveAsync();

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
