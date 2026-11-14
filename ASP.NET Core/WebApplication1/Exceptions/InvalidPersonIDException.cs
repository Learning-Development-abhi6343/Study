// <copyright file="InvalidPersonIDException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Exceptions
{
    /// <summary>
    /// Represents an error that occurs when a person ID is invalid.
    /// </summary>
    public class InvalidPersonIDException : ArgumentException
    {
        public InvalidPersonIDException() 
            : base("Invalid Person ID. Person ID must be greater than zero.")
        {
        }

        public InvalidPersonIDException(string? message)
            : base(message)
        {
        }

        public InvalidPersonIDException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
