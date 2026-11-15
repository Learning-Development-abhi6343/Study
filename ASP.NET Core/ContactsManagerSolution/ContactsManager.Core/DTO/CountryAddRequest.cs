// <copyright file="CountryAddRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.DTO
{
    using ContactsManager.Core.Domain.Entities;

    /// <summary>
    /// DTO class for adding a new country.
    /// </summary>
    public class CountryAddRequest
    {
        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string? CountryName { get; set; }

        /// <summary>
        /// Creates a country from the current request.
        /// </summary>
        /// <returns>A country with the same country name.</returns>
        public Country ToCountry()
        {
            return new() { CountryName = this.CountryName };
        }
    }
}
