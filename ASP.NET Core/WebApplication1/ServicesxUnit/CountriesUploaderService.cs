// <copyright file="CountriesUploaderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using Entities;
    using Microsoft.AspNetCore.Http;
    using OfficeOpenXml;
    using RepositoryContracts;
    using ServiceContractsxUnit;

    /// <summary>
    /// Uploads countries from the "Countries" worksheet of an Excel file, skipping blank names and countries that
    /// already exist.
    /// </summary>
    public class CountriesUploaderService : ICountriesUploaderService
    {
        /// <summary>
        /// Repository for accessing country data.
        /// </summary>
        readonly ICountriesRepository _countriesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesUploaderService"/> class.
        /// </summary>
        /// <param name="countriesRepository">The repository used to access country data.</param>
        public CountriesUploaderService(ICountriesRepository countriesRepository)
        {
            this._countriesRepository = countriesRepository;
        }

        /// <inheritdoc/>
        public async Task<int> UploadCountriesFromExcelFile(IFormFile formfile)
        {
            var memoryStream = new MemoryStream();
            await formfile.CopyToAsync(memoryStream);
            using var excelPackage = new ExcelPackage(memoryStream);
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];

            int rowCount = worksheet.Dimension.Rows;
            int countriesAdded = 0;

            for (int i = 2; i <= rowCount; i++)
            {
                var countryName = worksheet.Cells[i, 1].Value?.ToString();
                if (string.IsNullOrEmpty(countryName))
                {
                    continue;
                }

                if (await this._countriesRepository.GetCountryByCountryName(countryName) != null)
                {
                    continue;
                }

                var country = new Country
                {
                    CountryName = countryName,
                };

                await this._countriesRepository.AddCountry(country);

                countriesAdded++;
            }

            return countriesAdded;
        }
    }
}
