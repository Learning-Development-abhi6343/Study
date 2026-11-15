// <copyright file="PersonAddRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.DTO
{
    using System.ComponentModel.DataAnnotations;
    using ContactsManager.Core.Domain.Entities;
    using ContactsManager.Core.Enums;

    /// <summary>
    /// Acts as DTO for inserting a new Person.
    /// </summary>
    public class PersonAddRequest
    {
        /// <summary>
        /// Gets or sets the person's name.
        /// </summary>
        /// <remarks>A value is required; blank values are not accepted.</remarks>
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the selected country.
        /// </summary>
        [Required(ErrorMessage = "Please select a country")]
        public Guid? CountryID { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the person's gender.
        /// </summary>
        [Required(ErrorMessage = "Please select a gender of the person")]
        public GenderOptions Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the person's address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the person wants to receive newsletters.
        /// </summary>
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Creates a person from the request data.
        /// </summary>
        /// <returns>A person containing the request data, with the gender represented as a string.</returns>
        public Person ToPerson()
        {
            return new()
            {
                CountryID = this.CountryID,
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Address = this.Address,
                Gender = this.Gender.ToString(),
                ReceiveNewsLetters = this.ReceiveNewsLetters,
            };
        }
    }
}
