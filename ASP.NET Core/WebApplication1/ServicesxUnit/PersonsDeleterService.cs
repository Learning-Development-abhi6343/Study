// <copyright file="PersonsDeleterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using RepositoryContracts;
    using ServiceContractsxUnit;

    /// <summary>
    /// Deletes a person by ID.
    /// </summary>
    /// <remarks>Throws <see cref="ArgumentNullException"/> if <paramref name="personID"/> is <see langword="null"/>. Returns <see langword="false"/> if the person does not exist; otherwise, deletes the person and returns <see langword="true"/>.</remarks>
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
