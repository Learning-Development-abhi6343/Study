// <copyright file="Person.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Person domain model class.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the person ID.
        /// </summary>
        [Key]
        public Guid PersonID { get; set; }

        /// <summary>
        /// Gets or sets the country ID associated with the person.
        /// </summary>
        public Guid? CountryID { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        // nvarchar(max) in SQL Server, so string type is used here
        [StringLength(40)] // nvarchar(40) in SQL Server, so string type is used here
        public string? PersonName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the person.
        /// </summary>
        [StringLength(40)] // nvarchar(40) in SQL Server, so string type is used here
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the gender of the person.
        /// </summary>
        [StringLength(10)] // nvarchar(10) in SQL Server, so string type is used here
        public string? Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the person.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the address of the person.
        /// </summary>
        [StringLength(200)] // nvarchar(200) in SQL Server, so string type is used here
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the person wants to receive newsletters.
        /// </summary>

        // bit in SQL Server, so bool type is used here
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Gets or sets the Tax Identification Number (TIN) of the person.
        /// </summary>
        // [StringLength(20)] // nvarchar(20) in SQL Server, so string type is used here
        public string? TIN { get; set; }

        /// <summary>
        /// Gets or sets the country associated with the person.
        /// </summary>
        [ForeignKey("CountryID")]
        public virtual Country? Country { get; set; }

        /// <summary>
        /// Returns a string representation of the Person object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"PersonID: {this.PersonID}, CountryID: {this.CountryID}, PersonName: {this.PersonName}, Email: {this.Email}, Gender: {this.Gender}, DateOfBirth: {this.DateOfBirth}, Address: {this.Address}, ReceiveNewsLetters: {this.ReceiveNewsLetters}, TIN: {this.TIN}";
        }
    }
}
