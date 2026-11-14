// <copyright file="IPersonsGetterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit
{
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Provides operations for retrieving, searching, and exporting person data.
    /// </summary>
    public interface IPersonsGetterService
    {
        /// <summary>
        /// Retrieves all persons.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of person responses.</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Retrieves a person by ID.
        /// </summary>
        /// <param name="personID">The ID of the person to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the person response, or <see
        /// langword="null"/>.</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Retrieves persons matching the specified search criteria.
        /// </summary>
        /// <param name="searchBy">The person property to search by.</param>
        /// <param name="searchString">The value to search for, or <see langword="null"/> to omit the search value.</param>
        /// <returns>A task representing the asynchronous operation, containing the matching persons.</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Retrieves person data in CSV format.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the CSV data in a memory stream.</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Retrieves the persons data as an Excel workbook.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains the Excel workbook as a memory
        /// stream.</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
