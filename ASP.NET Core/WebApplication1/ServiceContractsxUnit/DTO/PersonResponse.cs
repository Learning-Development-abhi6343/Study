// <copyright file="PersonResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit.DTO
{
    using Entities;
    using ServiceContractsxUnit.Enums;

    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Person service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? Age { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The PersonResponse object to compare</param>
        /// <returns>True orFalse, indicating whether all person details are amatched with the specified parameter object</returns>
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

            var person = (PersonResponse) obj;
            return PersonID == person.PersonID && PersonName == person.PersonName && Email == person.Email && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryID == person.CountryID && Address == person.Address && ReceiveNewsLetters == person.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {Email}, Date Of Birth: {DateOfBirth?.ToString("dd MMM yyyy")}, Gender: {Gender}, Country ID: {CountryID}, Country: {Country}, Address: {Address}, Receive News Letters: {ReceiveNewsLetters}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                Address = Address,
                CountryID = CountryID,
                ReceiveNewsLetters = ReceiveNewsLetters,
            };
        }
    }

    public static class PersonExtensions
    {
        /// <summary>
        /// An extension method to convert an object of Person class to PersonResponse class
        /// </summary>
        /// <param name="person">The Person obejct to convert</param>
        /// <returns>Returns the converted Personresponse object</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            // Person => PersonResponse
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
                Country = person.Country?.CountryName
            };
        }

        public static PersonResponse ConvertPersonToPersonResponse(this Person person)
        {
            var personResponse = person.ToPersonResponse();

            //personResponse.Country = _countryService.GetCountryByCopuntryID(person.CountryID)?.CountryName;
            personResponse.Country = person.Country?.CountryName;
            return personResponse;
        }
    }
}
