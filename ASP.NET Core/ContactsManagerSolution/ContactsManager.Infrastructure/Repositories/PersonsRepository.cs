// <copyright file="PersonsRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Infrastructure.Repositories
{
    using System.Linq.Expressions;
    using ContactsManager.Core.Domain.Entities;
    using ContactsManager.Core.Domain.RepositoryContracts;
    using ContactsManager.Infrastructure.DbContexts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides data access operations for persons.
    /// </summary>
    public class PersonsRepository : IPersonsRepository
    {
        /// <summary>
        /// The database context used to access application data.
        /// </summary>
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Provides logging for the persons repository.
        /// </summary>
        private readonly ILogger<PersonsRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsRepository"/> class.
        /// </summary>
        /// <param name="db">The database context used to access application data.</param>
        /// <param name="logger">The logger used to record repository events.</param>
        public PersonsRepository(ApplicationDbContext db, ILogger<PersonsRepository> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        /// <summary>
        /// Adds a person and saves the changes.
        /// </summary>
        /// <param name="person">The person to add.</param>
        /// <returns>The added person.</returns>
        public async Task<Person> AddPerson(Person person)
        {
            this._db.Persons.Add(person);
            await this._db.SaveChangesAsync();
            return person;
        }

        /// <summary>
        /// Deletes the person with the specified ID.
        /// </summary>
        /// <param name="personID">The ID of the person to delete.</param>
        /// <returns>true if a person was deleted; otherwise, false.</returns>
        public async Task<bool> DeletePersonByPersonID(Guid personID)
        {
            this._db.Persons.RemoveRange(this._db.Persons.Where(temp => temp.PersonID == personID));
            int rowsDeleted = await this._db.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        /// <summary>
        /// Retrieves all persons, including their associated countries.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of persons.</returns>
        public Task<List<Person>> GetAllPersons()
        {
            this._logger.LogInformation("GetAllPersons of PersonsRepository");
            return this._db.Persons.Include(nameof(Person.Country)).ToListAsync();
        }

        /// <summary>
        /// Retrieves persons matching the specified predicate, including their countries.
        /// </summary>
        /// <param name="predicate">The condition used to filter persons.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching persons.</returns>
        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            this._logger.LogInformation("GetFilteredPersons of PersonsRepository");
            return await this._db.Persons.Include(nameof(Person.Country)).Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Retrieves a person by ID, including their country.
        /// </summary>
        /// <param name="personID">The ID of the person to retrieve.</param>
        /// <returns>The matching person, or null if no person is found.</returns>
        public async Task<Person?> GetPersonByPersonID(Guid personID)
        {
            return await this._db.Persons.Include(nameof(Person.Country)).FirstOrDefaultAsync(temp => temp.PersonID == personID);
        }

        /// <summary>
        /// Updates the person with the matching ID.
        /// </summary>
        /// <param name="person">The person containing the updated values.</param>
        /// <returns>The updated person, or the supplied person if no matching person exists.</returns>
        public async Task<Person> UpdatePerson(Person person)
        {
            var matchingPerson = await this._db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID);

            if (matchingPerson == null)
            {
                return person;
            }

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender;
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

            int countUpdated = await this._db.SaveChangesAsync();

            return matchingPerson;
        }
    }
}
