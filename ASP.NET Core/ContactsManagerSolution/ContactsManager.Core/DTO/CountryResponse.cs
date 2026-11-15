// <copyright file="CountryResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.DTO
{
    using ContactsManager.Core.Domain.Entities;

    /// <summary>
    /// DTO class that is used as return type for most CountriesService methods.
    /// </summary>
    public class CountryResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the country.
        /// </summary>
        public Guid CountryID { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string? CountryName { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>true if obj is a CountryResponse with matching CountryID and CountryName; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            var country_to_compare = (CountryResponse)obj;
            return this.CountryID == country_to_compare.CountryID && this.CountryName == country_to_compare.CountryName;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Provides extension methods for converting countries to response objects.
    /// </summary>
    public static class CountryExtensions
    {
        /// <summary>
        /// Creates a country response from a country.
        /// </summary>
        /// <param name="country">The country to convert.</param>
        /// <returns>A response containing the country's ID and name.</returns>
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new() { CountryID = country.CountryID, CountryName = country.CountryName };
        }
    }
}
