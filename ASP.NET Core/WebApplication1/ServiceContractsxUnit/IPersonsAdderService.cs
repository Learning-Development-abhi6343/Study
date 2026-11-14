// <copyright file="IPersonsAdderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit
{
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Provides an abstraction for adding people.
    /// </summary>
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a person.
        /// </summary>
        /// <param name="personAddRequest">The details of the person to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response for the added person.</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
    }
}
