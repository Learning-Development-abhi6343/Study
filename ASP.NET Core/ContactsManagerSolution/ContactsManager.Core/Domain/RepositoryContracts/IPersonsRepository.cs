// <copyright file="IPersonsRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    using System.Linq.Expressions;
    using ContactsManager.Core.Domain.Entities;

    /// <summary>
    /// Represents data access logic for managing Person entity.
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a person.
        /// </summary>
        /// <param name="person">The person to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added person.</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Retrieves all persons.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation and contains the list of persons.</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Retrieves a person by their ID.
        /// </summary>
        /// <param name="personID">The ID of the person to retrieve.</param>
        /// <returns>A task whose result is the person with the specified ID, or null if no person is found.</returns>
        Task<Person?> GetPersonByPersonID(Guid personID);

        /// <summary>
        /// Retrieves persons that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The expression used to filter persons.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching persons.</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes the person with the specified ID.
        /// </summary>
        /// <param name="personID">The ID of the person to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result is true if the person was deleted;
        /// otherwise, false.</returns>
        Task<bool> DeletePersonByPersonID(Guid personID);

        /// <summary>
        /// Updates an existing person record.
        /// </summary>
        /// <param name="person">The person details to update.</param>
        /// <returns>The updated person.</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
