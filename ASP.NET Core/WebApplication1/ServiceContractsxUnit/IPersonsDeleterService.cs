// <copyright file="IPersonsDeleterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit
{
    /// <summary>
    /// Provides an asynchronous operation for deleting a person.
    /// </summary>
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Deletes a person.
        /// </summary>
        /// <param name="personID">The identifier of the person to delete, or <see langword="null"/>.</param>
        /// <returns>A task representing the asynchronous operation. Its result is <see langword="true"/> if the person was
        /// deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeletePerson(Guid? personID);
    }
}
