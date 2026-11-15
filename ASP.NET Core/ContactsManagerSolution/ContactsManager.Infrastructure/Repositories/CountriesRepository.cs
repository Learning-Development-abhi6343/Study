// <copyright file="CountriesRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Infrastructure.Repositories
{
    using ContactsManager.Core.Domain.Entities;
    using ContactsManager.Core.Domain.RepositoryContracts;
    using ContactsManager.Infrastructure.DbContexts;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Represents a repository for managing countries in the database.
    /// </summary>
    public class CountriesRepository : ICountriesRepository
    {
        /// <summary>
        /// Gets the database context used for accessing the countries data.
        /// </summary>
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="db">The database context.</param>
        public CountriesRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        /// <summary>
        /// Adds a new country to the database.
        /// </summary>
        /// <param name="country">The country to add.</param>
        /// <returns>The added country.</returns>
        public async Task<Country> AddCountry(Country country)
        {
            this._db.Countries.Add(country);
            await this._db.SaveChangesAsync();

            return country;
        }

        /// <summary>
        /// Gets all countries from the database.
        /// </summary>
        /// <returns>A list of all countries.</returns>
        public async Task<List<Country>> GetAllCountries()
        {
            return await this._db.Countries.ToListAsync();
        }

        /// <summary>
        /// Gets a country from the database by its unique identifier (CountryID).
        /// </summary>
        /// <param name="countryID">The unique identifier of the country to retrieve.</param>
        /// <returns>The country if found, otherwise null.</returns>
        public async Task<Country?> GetCountryByCountryID(Guid countryID)
        {
            return await this._db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryID);
        }

        /// <summary>
        /// Gets a country from the database by its name.
        /// </summary>
        /// <param name="countryName">The name of the country to retrieve.</param>
        /// <returns>The country if found, otherwise null.</returns>
        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            return await this._db.Countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
        }
    }
}
