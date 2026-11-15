// <copyright file="Country.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Domain Model for Country.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Gets or sets the unique identifier for the country.
        /// </summary>
        [Key]
        public Guid CountryID { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string? CountryName { get; set; }

        /// <summary>
        /// Gets or sets the collection of persons from this country.
        /// </summary>
        public virtual ICollection<Person>? Persons { get; set; }
    }
}
