// <copyright file="CountriesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.UI.Controllers
{
    using ContactsManager.Core.ServiceContracts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Provides actions for displaying the country upload page and importing countries from an Excel file.
    /// </summary>
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        /// <summary>
        /// Provides access to country-related operations.
        /// </summary>
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ICountriesUploaderService _countriesUploaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesController"/> class.
        /// </summary>
        /// <param name="countriesGetterService">The service used to retrieve country data.</param>
        /// <param name="countriesUploaderService">The service used to upload country data.</param>
        public CountriesController(ICountriesGetterService countriesGetterService, ICountriesUploaderService countriesUploaderService)
        {
            this._countriesGetterService = countriesGetterService;
            this._countriesUploaderService = countriesUploaderService;
        }

        /// <summary>
        /// Displays the Excel upload page.
        /// </summary>
        /// <returns>The view result for the Excel upload page.</returns>
        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return this.View();
        }

        /// <summary>
        /// Uploads country data from an Excel workbook.
        /// </summary>
        /// <param name="excelFile">The Excel file to upload. The file must have the .xlsx extension.</param>
        /// <returns>A view displaying the upload result or an error message.</returns>
        [Route("UploadFromExcel")]
        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                this.ViewBag.ErrorMessage = "Please select a valid Excel file.";
                return this.View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                this.ViewBag.ErrorMessage = "Please select a valid Excel file with .xlsx extension."; 
                return this.View();
            }

            try
            {
                int countriesAdded = await this._countriesUploaderService.UploadCountriesFromExcelFile(excelFile);
                this.ViewBag.Message = $"{countriesAdded} countries added successfully.";
            }
            catch (Exception ex)
            {
                this.ViewBag.ErrorMessage = $"An error occurred while uploading: {ex.Message}";
            }

            return this.View();
        }
    }
}
