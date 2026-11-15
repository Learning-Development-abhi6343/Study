// <copyright file="CountriesGetterService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ServicesxUnit
{
    using RepositoryContracts;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// Provides access to country data.
    /// </summary>
    public class CountriesGetterService : ICountriesGetterService
    {
        /// <summary>
        /// Provides access to the countries repository.
        /// </summary>
        readonly ICountriesRepository _countriesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesGetterService"/> class.
        /// </summary>
        /// <param name="countriesRepository">The repository used to retrieve countries.</param>
        public CountriesGetterService(ICountriesRepository countriesRepository)
        {
            this._countriesRepository = countriesRepository;
        }

        /// <inheritdoc/>
        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return [.. (await this._countriesRepository.GetAllCountries()).Select(country => country.ToCountryResponse())];
        }

        /// <inheritdoc/>
        public async Task<CountryResponse?> GetCountryByCopuntryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }

            var country_response_from_list = await this._countriesRepository.GetCountryByCountryID(countryID.Value);
            if (country_response_from_list == null)
            {
                return null;
            }

            return country_response_from_list.ToCountryResponse();
        }
    }
}
