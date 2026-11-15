// <copyright file="CountriesServiceTestISP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.ServiceTests
{
    using AutoFixture;
    using ContactsManager.Core.Domain.Entities;
    using ContactsManager.Core.Domain.RepositoryContracts;
    using ContactsManager.Core.DTO;
    using ContactsManager.Core.ServiceContracts;
    using ContactsManager.Core.Services;
    using FluentAssertions;
    using Moq;

    public class CountriesServiceTestISP
    {
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ICountriesAdderService _countriesAdderService;

        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;

        private readonly IFixture _fixture;

        public CountriesServiceTestISP()
        {
            this._fixture = new Fixture();
            this._countriesRepositoryMock = new Mock<ICountriesRepository>();
            this._countriesRepository = this._countriesRepositoryMock.Object;

            this._countriesGetterService = new CountriesGetterService(this._countriesRepository);
            this._countriesAdderService = new CountriesAdderService(this._countriesRepository);
        }

        #region AddCountry

        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry_ToBeArgumentNullException()
        {
            // Arrange
            CountryAddRequest? request = null;

            Country country = this._fixture.Build<Country>()
                 .With(temp => temp.Persons, null as List<Person>).Create();

            this._countriesRepositoryMock
             .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(country);


            // Act
            var action = async () =>
            {
                await this._countriesAdderService.AddCountry(request);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }


        // When the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull_ToBeArgumentException()
        {
            // Arrange
            CountryAddRequest? request = this._fixture.Build<CountryAddRequest>()
             .With(temp => temp.CountryName, null as string)
             .Create();

            Country country = this._fixture.Build<Country>()
                 .With(temp => temp.Persons, null as List<Person>).Create();

            this._countriesRepositoryMock
             .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(country);

            // Act
            var action = async () =>
            {
                await this._countriesAdderService.AddCountry(request);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        // When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName_ToBeArgumentException()
        {
            // Arrange
            CountryAddRequest first_country_request = this._fixture.Build<CountryAddRequest>()
                 .With(temp => temp.CountryName, "Test name").Create();
            CountryAddRequest second_country_request = this._fixture.Build<CountryAddRequest>()
              .With(temp => temp.CountryName, "Test name").Create();

            Country first_country = first_country_request.ToCountry();
            Country second_country = second_country_request.ToCountry();

            this._countriesRepositoryMock
             .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(first_country);

            // Return null when GetCountryByCountryName is called
            this._countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
             .ReturnsAsync(null as Country);

            CountryResponse first_country_from_add_country = await this._countriesAdderService.AddCountry(first_country_request);

            // Act
            var action = async () =>
            {
                // Return first country when GetCountryByCountryName is called
                this._countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(first_country);

                this._countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>())).ReturnsAsync(first_country);

                await this._countriesAdderService.AddCountry(second_country_request);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        // When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public async Task AddCountry_FullCountry_ToBeSuccessful()
        {
            // Arrange
            var country_request = this._fixture.Create<CountryAddRequest>();
            var country = country_request.ToCountry();
            var country_response = country.ToCountryResponse();

            this._countriesRepositoryMock
             .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(country);

            this._countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
             .ReturnsAsync(null as Country);

            // Act
            var country_from_add_country = await this._countriesAdderService.AddCountry(country_request);

            country.CountryID = country_from_add_country.CountryID;
            country_response.CountryID = country_from_add_country.CountryID;

            // Assert
            country_from_add_country.CountryID.Should().NotBe(Guid.Empty);
            country_from_add_country.Should().BeEquivalentTo(country_response);
        }

        #endregion


        #region GetAllCountries

        // The list of countries should be empty by default (before adding any countries)
        [Fact]
        public async Task GetAllCountries_ToBeEmptyList()
        {
            // Arrange
            List<Country> country_empty_list = [];
            this._countriesRepositoryMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(country_empty_list);

            // Act
            var actual_country_response_list = await this._countriesGetterService.GetAllCountries();

            // Assert
            actual_country_response_list.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCountries_ShouldHaveFewCountries()
        {
            // Arrange
            List<Country> country_list =
            [
                this._fixture.Build<Country>()
                .With(temp => temp.Persons, null as List<Person>).Create(),
                this._fixture.Build<Country>()
                .With(temp => temp.Persons, null as List<Person>).Create(),
            ];

            var country_response_list = country_list.ConvertAll(temp => temp.ToCountryResponse());

            this._countriesRepositoryMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(country_list);

            // Act
            var actualCountryResponseList = await this._countriesGetterService.GetAllCountries();

            // Assert
            actualCountryResponseList.Should().BeEquivalentTo(country_response_list);
        }
        #endregion


        #region GetCountryByCountryID

        // If we supply null as CountryID, it should return null as CountryResponse
        [Fact]
        public async Task GetCountryByCountryID_NullCountryID_ToBeNull()
        {
            // Arrange
            Guid? countryID = null;

            this._countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
             .ReturnsAsync(null as Country);

            // Act
            var country_response_from_get_method = await this._countriesGetterService.GetCountryByCountryID(countryID);

            // Assert
            country_response_from_get_method.Should().BeNull();
        }

        // If we supply a valid country id, it should return the matching country details as CountryResponse object
        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID_ToBeSuccessful()
        {
            // Arrange
            var country = this._fixture.Build<Country>()
              .With(temp => temp.Persons, null as List<Person>)
              .Create();
            var country_response = country.ToCountryResponse();

            this._countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
             .ReturnsAsync(country);

            // Act
            var country_response_from_get = await this._countriesGetterService.GetCountryByCountryID(country.CountryID);

            // Assert
            country_response_from_get.Should().Be(country_response);
        }
        #endregion
    }
}
