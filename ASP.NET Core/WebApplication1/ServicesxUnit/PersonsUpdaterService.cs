// <copyright file="PersonsUpdaterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using Exceptions;
    using RepositoryContracts;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;
    using ServicesxUnit.Helpers;

    /// <summary>
    /// Updates person records using a persons repository.
    /// </summary>
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        /// <summary>
        /// Repository for accessing person data.
        /// </summary>
        private readonly IPersonsRepository _personsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsUpdaterService"/> class.
        /// </summary>
        /// <param name="personsRepository">The repository for managing persons.</param>
        public PersonsUpdaterService(IPersonsRepository personsRepository)
        {
            this._personsRepository = personsRepository;
        }

        /// <inheritdoc/>
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            ArgumentNullException.ThrowIfNull(personUpdateRequest);

            ValidationHelper.ModelValidation(personUpdateRequest);

            var matchingPerson = await this._personsRepository.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                 throw new InvalidPersonIDException("Given person id doesn't exist");
            }

            matchingPerson!.PersonName = personUpdateRequest.PersonName;
            matchingPerson!.Email = personUpdateRequest.Email;
            matchingPerson!.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson!.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson!.CountryID = personUpdateRequest.CountryID;
            matchingPerson!.Address = personUpdateRequest.Address;
            matchingPerson!.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            await this._personsRepository.UpdatePerson(matchingPerson!);

            return matchingPerson!.ToPersonResponse();
        }
    }
}
