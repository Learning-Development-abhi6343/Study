// <copyright file="InvalidPersonIDException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when a person ID is invalid.
    /// </summary>
    public class InvalidPersonIDException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPersonIDException"/> class.
        /// </summary>
        public InvalidPersonIDException()
            : base("Invalid Person ID. Person ID must be greater than zero.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPersonIDException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidPersonIDException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPersonIDException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public InvalidPersonIDException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
