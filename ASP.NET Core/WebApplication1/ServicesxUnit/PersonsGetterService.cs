// <copyright file="PersonsGetterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using CsvHelper;
    using Entities;
    using Microsoft.Extensions.Logging;
    using OfficeOpenXml;
    using RepositoryContracts;
    using Serilog;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Provides methods for retrieving, filtering, and exporting person records.
    /// </summary>
    /// <remarks>Supports exporting person records as CSV or Excel files.</remarks>
    public class PersonsGetterService : IPersonsGetterService
    {
        /// <summary>
        /// Repository for accessing person data.
        /// </summary>
        readonly IPersonsRepository _personsRepository;

        /// <summary>
        /// Logger used to record diagnostic information for the persons service.
        /// </summary>
        readonly ILogger<PersonsService> _logger;

        /// <summary>
        /// Provides diagnostic context for recording request-specific properties.
        /// </summary>
        readonly IDiagnosticContext _diagnosticContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsGetterService"/> class.
        /// </summary>
        /// <param name="personsRepository">The repository for managing persons.</param>
        /// <param name="logger">The logger for logging information.</param>
        /// <param name="diagnosticContext">The diagnostic context for collecting diagnostic information.</param>
        public PersonsGetterService(IPersonsRepository personsRepository, ILogger<PersonsService> logger, IDiagnosticContext diagnosticContext)
        {
            this._personsRepository = personsRepository;
            this._logger = logger;
            this._diagnosticContext = diagnosticContext;
        }

        /// <inheritdoc/>
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await this._personsRepository.GetAllPersons();
            this._logger.LogInformation("GetAllPersons of PersonsGetterService");
            return [.. persons.Select(temp => temp.ToPersonResponse())];
        }

        /// <inheritdoc/>
        // public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        public virtual async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null;
            }

            var person = await this._personsRepository.GetPersonByPersonID(personID.Value);

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        /// <inheritdoc/>
        // public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        public virtual async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            this._logger.LogInformation("GetFilteredPersons of PersonsService");
            var matchingPersons = new List<Person>();
            using (SerilogTimings.Operation.Time("Time taken to get filtered persons from database"))
            {
                matchingPersons = searchBy switch
                {
                    nameof(PersonResponse.PersonName) => [.. await this._personsRepository.GetFilteredPersons(temp => temp.PersonName!.Contains(searchString ?? string.Empty))],

                    nameof(PersonResponse.Email) => [.. await this._personsRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(temp.Email) || temp.Email.Contains(searchString ?? string.Empty))],

                    nameof(PersonResponse.DateOfBirth) => [.. await this._personsRepository.GetFilteredPersons(temp => temp.DateOfBirth == null || temp.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString ?? string.Empty))],

                    nameof(PersonResponse.Gender) => [.. await this._personsRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(temp.Gender) || temp.Gender.Equals(searchString ?? string.Empty))],

                    nameof(PersonResponse.Address) => [.. await this._personsRepository.GetFilteredPersons(temp => string.IsNullOrEmpty(temp.Address) || temp.Address.Contains(searchString ?? string.Empty))],

                    _ => await this._personsRepository.GetAllPersons(),
                };
            }

            this._diagnosticContext.Set("MatchingPersons", matchingPersons);
            return [.. matchingPersons.Select(temp => temp.ToPersonResponse())];
        }

        /// <inheritdoc/>
        // public async Task<MemoryStream> GetPersonsCSV()
        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            var memoryStream = new MemoryStream();
            await using (var streamWriter = new StreamWriter(memoryStream, leaveOpen: true))
            await using (var csvWriter = CsvWriter.Create(streamWriter, leaveOpen: true))
            {
                csvWriter.WriteField(nameof(PersonResponse.PersonName));
                csvWriter.WriteField(nameof(PersonResponse.Email));
                csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
                csvWriter.WriteField(nameof(PersonResponse.Age));
                csvWriter.WriteField(nameof(PersonResponse.Gender));
                csvWriter.WriteField(nameof(PersonResponse.Country));
                csvWriter.WriteField(nameof(PersonResponse.Address));
                csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
                csvWriter.EndRecord();

                var persons = await this.GetAllPersons();
                foreach (var person in persons)
                {
                    csvWriter.WriteField(person.PersonName);
                    csvWriter.WriteField(person.Email);
                    csvWriter.WriteField(person.DateOfBirth?.ToString("dd MMM yyyy"));
                    csvWriter.WriteField(person.Age);
                    csvWriter.WriteField(person.Gender);
                    csvWriter.WriteField(person.Country);
                    csvWriter.WriteField(person.Address);
                    csvWriter.WriteField(person.ReceiveNewsLetters);
                    csvWriter.EndRecord();
                }
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        /// <inheritdoc/>
        // public async Task<MemoryStream> GetPersonsExcel()
        public virtual async Task<MemoryStream> GetPersonsExcel()
        {
            var memoryStream = new MemoryStream();
            using var excelPackage = new ExcelPackage(memoryStream);
            var workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
            workSheet.Cells["A1"].Value = "Person Name";
            workSheet.Cells["B1"].Value = "Email";
            workSheet.Cells["C1"].Value = "Date of Birth";
            workSheet.Cells["D1"].Value = "Age";
            workSheet.Cells["E1"].Value = "Gender";
            workSheet.Cells["F1"].Value = "Country";
            workSheet.Cells["G1"].Value = "Address";
            workSheet.Cells["H1"].Value = "Receive News Letters";

            using var headerCells = workSheet.Cells["A1:H1"];
            headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            headerCells.Style.Font.Bold = true;

            int row = 2;
            var persons = await this.GetAllPersons();
            foreach (var person in persons)
            {
                workSheet.Cells[$"A{row}"].Value = person.PersonName;
                workSheet.Cells[$"B{row}"].Value = person.Email;
                workSheet.Cells[$"C{row}"].Value = person.DateOfBirth?.ToString("dd MMM yyyy");
                workSheet.Cells[$"D{row}"].Value = person.Age;
                workSheet.Cells[$"E{row}"].Value = person.Gender;
                workSheet.Cells[$"F{row}"].Value = person.Country;
                workSheet.Cells[$"G{row}"].Value = person.Address;
                workSheet.Cells[$"H{row}"].Value = person.ReceiveNewsLetters;
                row++;
            }

            workSheet.Cells[$"A1:H{row}"].AutoFitColumns();
            await excelPackage.SaveAsync();

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
