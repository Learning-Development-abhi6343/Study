// <copyright file="PersonsAdderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using RepositoryContracts;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;
    using ServicesxUnit.Helpers;

    /// <summary>
    /// Adds a person to the repository after validating the request.
    /// </summary>
    public class PersonsAdderService : IPersonsAdderService
    {
        /// <summary>
        /// Repository for accessing person data.
        /// </summary>
        readonly IPersonsRepository _personsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsAdderService"/> class.
        /// </summary>
        /// <param name="personsRepository">The repository for managing persons.</param>
        public PersonsAdderService(IPersonsRepository personsRepository)
        {
            this._personsRepository = personsRepository;
        }

        /// <inheritdoc/>
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            ArgumentNullException.ThrowIfNull(personAddRequest);

            ValidationHelper.ModelValidation(personAddRequest);

            var person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();

            await this._personsRepository.AddPerson(person);

            return person.ToPersonResponse();
        }
    }
}
