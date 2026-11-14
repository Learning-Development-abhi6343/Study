// <copyright file="PersonsGetterServiceWithFewExcelFields.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using OfficeOpenXml;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Retrieves person data and exports selected fields to an Excel workbook.
    /// </summary>
    public class PersonsGetterServiceWithFewExcelFields : IPersonsGetterService
    {
        /// <summary>
        /// Gets persons using the persons getter service.
        /// </summary>
        private readonly PersonsGetterService _personsGetterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsGetterServiceWithFewExcelFields"/> class.
        /// </summary>
        /// <param name="personsGetterService">The service used to retrieve persons.</param>
        public PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
        {
            this._personsGetterService = personsGetterService;
        }

        /// <inheritdoc/>
        public async Task<List<PersonResponse>> GetAllPersons()
        {
           return await _personsGetterService.GetAllPersons();
        }

        /// <inheritdoc/>
        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            return await _personsGetterService.GetFilteredPersons(searchBy, searchString);
        }

        /// <inheritdoc/>
        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            return await _personsGetterService.GetPersonByPersonID(personID);
        }

        /// <inheritdoc/>
        public async Task<MemoryStream> GetPersonsCSV()
        {
            return await _personsGetterService.GetPersonsCSV();
        }

        /// <inheritdoc/>
        public async Task<MemoryStream> GetPersonsExcel()
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
