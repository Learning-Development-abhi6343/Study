// <copyright file="IPersonsUpdaterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit
{
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Provides an operation for updating a person's details.
    /// </summary>
    public interface IPersonsUpdaterService
    {
        /// <summary>
        /// Updates a person.
        /// </summary>
        /// <param name="personUpdateRequest">The person update request, or `null`.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the person response.</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}
