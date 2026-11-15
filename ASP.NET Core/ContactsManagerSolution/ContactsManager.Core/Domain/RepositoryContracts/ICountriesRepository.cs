// <copyright file="ICountriesRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    using ContactsManager.Core.Domain.Entities;

    /// <summary>
    /// Represents data access logic for managing Country entity.
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a country to the repository.
        /// </summary>
        /// <param name="country">The country to add.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the added country.</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Retrieves all countries.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation and contains the list of countries.</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Retrieves a country by its identifier.
        /// </summary>
        /// <param name="countryID">The identifier of the country to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the country if found; otherwise,
        /// <see langword="null"/>.</returns>
        Task<Country?> GetCountryByCountryID(Guid countryID);

        /// <summary>
        /// Gets a country by its name.
        /// </summary>
        /// <param name="countryName">The name of the country to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the country, or null if no
        /// matching country is found.</returns>
        Task<Country?> GetCountryByCountryName(string countryName);
    }
}
