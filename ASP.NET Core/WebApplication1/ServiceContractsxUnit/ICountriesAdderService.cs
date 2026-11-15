// <copyright file="ICountriesAdderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit
{
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Provides an asynchronous operation for adding a country.
    /// </summary>
    public interface ICountriesAdderService
    {
        /// <summary>
        /// Adds a country.
        /// </summary>
        /// <param name="countryAddRequest">The country details to add, or <see langword="null" />.</param>
        /// <returns>A task representing the asynchronous operation, with the country response.</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
    }
}
