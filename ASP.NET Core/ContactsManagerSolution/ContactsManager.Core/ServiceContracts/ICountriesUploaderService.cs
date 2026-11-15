// <copyright file="ICountriesUploaderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.ServiceContracts
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Uploads countries from an Excel file.
    /// </summary>
    public interface ICountriesUploaderService
    {
        /// <summary>
        /// Uploads countries from an Excel file.
        /// </summary>
        /// <param name="formfile">The Excel file containing the countries to upload.</param>
        /// <returns>A task whose result is the number of countries uploaded.</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formfile);
    }
}
