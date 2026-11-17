// <copyright file="LoginDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.DTO
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the data transfer object for user login information, including email and password.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the email address of the user attempting to log in. This property is required and must be in a valid email address format.
        /// </summary>
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to log in. This property is required and should be treated as sensitive information.
        /// </summary>
        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
