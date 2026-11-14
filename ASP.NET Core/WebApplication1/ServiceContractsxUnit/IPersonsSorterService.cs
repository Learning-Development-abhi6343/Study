// <copyright file="IPersonsSorterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServiceContractsxUnit
{
    using ServiceContractsxUnit.DTO;
    using ServiceContractsxUnit.Enums;

    /// <summary>
    /// Provides a service for sorting persons by a specified property and sort order.
    /// </summary>
    public interface IPersonsSorterService
    {
        /// <summary>
        /// Returns the persons sorted by the specified property and sort order.
        /// </summary>
        /// <param name="allPersons">The persons to sort.</param>
        /// <param name="sortBy">The property used to sort the persons.</param>
        /// <param name="sortOrder">The sort order to apply.</param>
        /// <returns>A task representing the asynchronous operation, containing the sorted persons.</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
    }
}
