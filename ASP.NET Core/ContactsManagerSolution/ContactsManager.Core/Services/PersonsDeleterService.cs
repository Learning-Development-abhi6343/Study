// <copyright file="PersonsDeleterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.Services
{
    using ContactsManager.Core.Domain.RepositoryContracts;
    using ContactsManager.Core.ServiceContracts;

    /// <summary>
    /// Deletes person records using a persons repository.
    /// </summary>
    public class PersonsDeleterService : IPersonsDeleterService
    {
        /// <summary>
        /// Repository used to access person records.
        /// </summary>
        private readonly IPersonsRepository _personsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsDeleterService"/> class.
        /// </summary>
        /// <param name="personsRepository">The repository for managing persons.</param>
        public PersonsDeleterService(IPersonsRepository personsRepository)
        {
            this._personsRepository = personsRepository;
        }

        /// <inheritdoc/>
        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            var person = await this._personsRepository.GetPersonByPersonID(personID.Value);
            if (person == null)
            {
                return false;
            }

            await this._personsRepository.DeletePersonByPersonID(personID.Value);
            return true;
        }
    }
}
