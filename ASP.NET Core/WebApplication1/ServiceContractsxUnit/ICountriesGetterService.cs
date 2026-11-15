// <copyright file="ICountriesGetterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit
{
    using System;
    using System.Collections.Generic;
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Provides operations for retrieving countries.
    /// </summary>
    public interface ICountriesGetterService
    {
        /// <summary>
        /// Gets all countries.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of countries.</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Retrieves a country by its identifier.
        /// </summary>
        /// <param name="countryID">The identifier of the country, or null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the country response, or null.</returns>
        Task<CountryResponse?> GetCountryByCopuntryID(Guid? countryID);
    }
}
