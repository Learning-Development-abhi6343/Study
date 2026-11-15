// <copyright file="PersonUpdateRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.DTO
{
    using System.ComponentModel.DataAnnotations;
    using ContactsManager.Core.Domain.Entities;
    using ContactsManager.Core.Enums;

    /// <summary>
    /// Represents the DTO class that contains the person details to update.
    /// </summary>
    public class PersonUpdateRequest
    {
        /// <summary>
        /// Gets or sets the person's unique identifier.
        /// </summary>
        [Required(ErrorMessage = "Person ID can't be blank")]
        public Guid PersonID { get; set; }

        /// <summary>
        /// Gets or sets the person's name.
        /// </summary>
        /// <remarks>The value is required and cannot be empty or consist only of white-space
        /// characters.</remarks>
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        /// <summary>
        /// Gets or sets the country identifier.
        /// </summary>
        public Guid? CountryID { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <remarks>The value is required and must be a valid email address.</remarks>
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the person's gender.
        /// </summary>
        public GenderOptions Gender { get; set; }

        /// <summary>
        /// Gets or sets the person's date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the person wants to receive newsletters.
        /// </summary>
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Converts the current object of the PersonAddRequest into a new object of Person type.
        /// </summary>
        /// <returns>Returns Person object.</returns>
        public Person ToPerson()
        {
            return new()
            {
                PersonID = this.PersonID,
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
