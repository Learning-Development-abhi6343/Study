// <copyright file="ICountriesService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit
{
    using Microsoft.AspNetCore.Http;
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Respresents the business logic for manipulating Country entity.
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries.
        /// </summary>
        /// <param name="countryAddRequest">Country object to be added.</param>
        /// <returns>Returns the country object after adding it  (including newly generated country id).</returns>

        // CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries from the list.
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse.</returns>

        // List<CountryResponse> GetAllCountries();
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given country id.
        /// </summary>
        /// <param name="countryID">CountryID (Guid) to search.</param>
        /// <returns>Matching country as CountryResponse object.</returns>

        // CountryResponse? GetCountryByCopuntryID(Guid? countryID);
        Task<CountryResponse?> GetCountryByCopuntryID(Guid? countryID);

        /// <summary>
        /// Uploads countries from the given excel file and returns the number of countries added to the list.
        /// </summary>
        /// <param name="formfile">The excel file containing country data.</param>
        /// <returns>The number of countries added.</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formfile);
    }
}
