// <copyright file="RegisterDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.DTO
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the data transfer object for user registration, containing properties for person name, email, phone number, password, and confirm password.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// Gets or sets the name of the person registering. This property is required and cannot be blank.
        /// </summary>
        [Required(ErrorMessage = "Name can't be blank.")]
        public string PersonName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the person registering. This property is required, must be in a proper email address format, and cannot be blank.
        /// </summary>
        [Required(ErrorMessage = "Email can't be blank.")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the person registering. This property is required, must contain only numbers, and cannot be blank.
        /// </summary>
        [Required(ErrorMessage = "Phone can't be blank.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain numbers only.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the password for the account being registered. This property is required, must be in a proper password format, and cannot be blank.
        /// </summary>
        [Required(ErrorMessage = "Password can't be blank.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmation password for the account being registered. This property is required, must match the password, and cannot be blank.
        /// </summary>
        [Required(ErrorMessage = "Confirm Password can't be blank.")]
        [DataType(DataType.Password)]

        // [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
