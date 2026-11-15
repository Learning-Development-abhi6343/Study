// <copyright file="PersonResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.DTO
{
    using ContactsManager.Core.Domain.Entities;
    using ContactsManager.Core.Enums;

    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Person service.
    /// </summary>
    public class PersonResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the person.
        /// </summary>
        public Guid PersonID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the country.
        /// </summary>
        public Guid? CountryID { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the person's name.
        /// </summary>
        public string? PersonName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the person's gender.
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        public double? Age { get; set; }

        /// <summary>
        /// Gets or sets the person's address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the person wants to receive newsletters.
        /// </summary>
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object.
        /// </summary>
        /// <param name="obj">The PersonResponse object to compare.</param>
        /// <returns>True orFalse, indicating whether all person details are amatched with the specified parameter object.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }

            var person = (PersonResponse)obj;
            return this.PersonID == person.PersonID && this.PersonName == person.PersonName && this.Email == person.Email && this.DateOfBirth == person.DateOfBirth && this.Gender == person.Gender && this.CountryID == person.CountryID && this.Address == person.Address && this.ReceiveNewsLetters == person.ReceiveNewsLetters;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of the person.
        /// </summary>
        /// <returns>A formatted string containing the person's details.</returns>
        public override string ToString()
        {
            return $"Person ID: {this.PersonID}, Person Name: {this.PersonName}, Email: {this.Email}, Date Of Birth: {this.DateOfBirth?.ToString("dd MMM yyyy")}, Gender: {this.Gender}, Country ID: {this.CountryID}, Country: {this.Country}, Address: {this.Address}, Receive News Letters: {this.ReceiveNewsLetters}";
        }

        /// <summary>
        /// Creates a person update request from the current person's data.
        /// </summary>
        /// <returns>A person update request populated with the person's details.</returns>
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new()
            {
                PersonID = this.PersonID,
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = Enum.Parse<GenderOptions>(this.Gender ?? string.Empty, true),
                Address = this.Address,
                CountryID = this.CountryID,
                ReceiveNewsLetters = this.ReceiveNewsLetters,
            };
        }
    }

    /// <summary>
    /// Provides extension methods for converting a Person to a PersonResponse.
    /// </summary>
    public static class PersonExtensions
    {
        /// <summary>
        /// An extension method to convert an object of Person class to PersonResponse class.
        /// </summary>
        /// <param name="person">The Person obejct to convert.</param>
        /// <returns>Returns the converted Personresponse object.</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                CountryID = person.CountryID,
                Gender = person.Gender,
                Address = person.Address,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Country = person.Country?.CountryName,
            };
        }

        /// <summary>
        /// Converts a person to a response, including the person's country name.
        /// </summary>
        /// <param name="person">The person to convert.</param>
        /// <returns>A response representing the person.</returns>
        public static PersonResponse ConvertPersonToPersonResponse(this Person person)
        {
            var personResponse = person.ToPersonResponse();

            personResponse.Country = person.Country?.CountryName;
            return personResponse;
        }
    }
}
