// <copyright file="PersonsControllerTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using CRUDExample.Controllers;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;
    using ServiceContractsxUnit.Enums;

    /// <summary>
    /// Contains unit tests for the PersonsController's Index and Create actions.
    /// </summary>
    public class PersonsControllerTest
    {
        /// <summary>
        /// Provides access to person-related operations.
        /// </summary>
        private readonly IPersonsService _personsService;

        /// <summary>
        /// Provides access to country-related operations.
        /// </summary>
        private readonly ICountriesService _countriesService;

        /// <summary>
        /// Mock of the persons service used by controller tests.
        /// </summary>
        private readonly Mock<IPersonsService> _personsServiceMock;

        /// <summary>
        /// Mock used to configure and verify interactions with the countries service.
        /// </summary>
        private readonly Mock<ICountriesService> _countriesServiceMock;

        /// <summary>
        /// Provides the fixture used by the test class.
        /// </summary>
        private readonly Fixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsControllerTest"/> class with a fixture and mocked
        /// service dependencies.
        /// </summary>
        public PersonsControllerTest()
        {
            this._fixture = new Fixture();

            this._countriesServiceMock = new Mock<ICountriesService>();
            this._personsServiceMock = new Mock<IPersonsService>();

            this._countriesService = this._countriesServiceMock.Object;
            this._personsService = this._personsServiceMock.Object;
        }

        #region Index

        /// <summary>
        /// Returns the Index view with a list of persons.
        /// </summary>
        /// <returns>A view result whose model contains the person responses.</returns>
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            // Arrange
            var persons_response_list = this._fixture.Create<List<PersonResponse>>();

            var personsController = new PersonsController(this._personsService, this._countriesService, Mock.Of<ILogger<PersonsController>>());

            this._personsServiceMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(persons_response_list);

            this._personsServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(persons_response_list);

            // Act
            var result = await personsController.Index(this._fixture.Create<string>(), this._fixture.Create<string>(), this._fixture.Create<string>(), this._fixture.Create<SortOrderOptions>());


            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(persons_response_list);
        }
        #endregion

        #region Create

        /// <summary>
        /// Returns the Create view with the original request when model validation fails.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task Create_IfModelErrors_ToReturnCreateView()
        {
            // Arrange
            var persons_add_request = this._fixture.Create<PersonAddRequest>();
            var person_response = this._fixture.Create<PersonResponse>();
            List<CountryResponse> countries = this._fixture.Create<List<CountryResponse>>();

            var personsController = new PersonsController(this._personsService, this._countriesService, Mock.Of<ILogger<PersonsController>>());

            this._countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);
            this._personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

            // Act
            personsController.ModelState.AddModelError("PersonName", "Person Name can't be blank");
            var result = await personsController.Create(persons_add_request);

            // Assert
            // Before adding filters
            /*var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(persons_add_request);*/

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }

        /// <summary>
        /// Verifies that creating a person without model errors redirects to the Index action.
        /// </summary>
        /// <returns>A task that represents the asynchronous test operation.</returns>
        [Fact]
        public async Task Create_IfNoModelErrors_ToReturnRedirectToIndexView()
        {
            // Arrange
            var persons_add_request = this._fixture.Create<PersonAddRequest>();
            var person_response = this._fixture.Create<PersonResponse>();
            List<CountryResponse> countries = this._fixture.Create<List<CountryResponse>>();

            var personsController = new PersonsController(this._personsService, this._countriesService, Mock.Of<ILogger<PersonsController>>());

            this._countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);
            this._personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

            // Act
            var result = await personsController.Create(persons_add_request);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}
