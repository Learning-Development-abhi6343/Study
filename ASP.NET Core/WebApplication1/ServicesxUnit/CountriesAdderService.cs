// <copyright file="CountriesAdderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using RepositoryContracts;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Adds countries to the country repository and returns a response for the added country.
    /// </summary>
    public class CountriesAdderService : ICountriesAdderService
    {
        /// <summary>
        /// Provides access to country data.
        /// </summary>
        readonly ICountriesRepository _countriesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesAdderService"/> class.
        /// </summary>
        /// <param name="countriesRepository">The repository used to access countries.</param>
        public CountriesAdderService(ICountriesRepository countriesRepository)
        {
            this._countriesRepository = countriesRepository;
        }

        /// <inheritdoc/>
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            ArgumentNullException.ThrowIfNull(countryAddRequest);

            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (await this._countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Given country name already exists");
            }

            var country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            await this._countriesRepository.AddCountry(country);

            return country.ToCountryResponse();
        }
    }
}
