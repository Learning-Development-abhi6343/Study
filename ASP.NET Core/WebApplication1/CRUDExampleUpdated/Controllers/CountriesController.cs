// <copyright file="CountriesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServiceContractsxUnit;

    /// <summary>
    /// Provides endpoints for displaying the country upload view and importing countries from Excel workbooks.
    /// </summary>
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        /// <summary>
        /// Service used to retrieve country data.
        /// </summary>
        private readonly ICountriesGetterService _countriesService;

        /// <summary>
        /// Provides access to the service for uploading countries.
        /// </summary>
        private readonly ICountriesUploaderService _countriesUploaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesController"/> class.
        /// </summary>
        /// <param name="countriesService">The service for retrieving countries.</param>
        /// <param name="countriesUploaderService">The service for uploading countries.</param>
        public CountriesController(ICountriesGetterService countriesService, ICountriesUploaderService countriesUploaderService)
        {
            this._countriesService = countriesService;
            this._countriesUploaderService = countriesUploaderService;
        }

        /// <summary>
        /// Displays the Excel upload view.
        /// </summary>
        /// <returns>An action result that renders the view.</returns>
        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return this.View();
        }

        /// <summary>
        /// Uploads countries from an Excel workbook.
        /// </summary>
        /// <param name="excelFile">The .xlsx file containing country data.</param>
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
                this.ViewBag.ErrorMessage = "Please select a valid Excel file with .xlsx extension."; // "UnSupported file. 'xlsx file is expected";
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
