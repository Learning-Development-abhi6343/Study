// <copyright file="PersonsControllerTestISP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using CRUDExampleUpdated.Controllers;
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
    public class PersonsControllerTestISP
    {
        /// <summary>
        /// Provides access to the service used to retrieve persons.
        /// </summary>
        private readonly IPersonsGetterService _personsGetterService;

        /// <summary>
        /// Service used to add persons.
        /// </summary>
        private readonly IPersonsAdderService _personsAdderService;

        /// <summary>
        /// Provides the service used to update persons.
        /// </summary>
        private readonly IPersonsUpdaterService _personsUpdaterService;

        /// <summary>
        /// The service used to delete persons.
        /// </summary>
        private readonly IPersonsDeleterService _personsDeleterService;

        /// <summary>
        /// Service for sorting persons.
        /// </summary>
        private readonly IPersonsSorterService _personsSorterService;

        /// <summary>
        /// Provides access to country-related operations.
        /// </summary>
        private readonly ICountriesService _countriesService;

        /// <summary>
        /// Logger for diagnostic messages from the persons controller.
        /// </summary>
        private readonly ILogger<PersonsController> _logger;

        /// <summary>
        /// Mock used to configure and verify the persons getter service.
        /// </summary>
        private readonly Mock<IPersonsGetterService> _personsGetterServiceMock;

        /// <summary>
        /// Mock used to configure and verify the persons adder service.
        /// </summary>
        private readonly Mock<IPersonsAdderService> _personsAdderServiceMock;

        /// <summary>
        /// Mock used to configure and verify the persons updater service.
        /// </summary>
        private readonly Mock<IPersonsUpdaterService> _personsUpdaterServiceMock;

        /// <summary>
        /// Provides a mock persons deleter service for tests.
        /// </summary>
        private readonly Mock<IPersonsDeleterService> _personsDeleterServiceMock;

        /// <summary>
        /// Mock for the persons sorter service used in tests.
        /// </summary>
        private readonly Mock<IPersonsSorterService> _personsSorterServiceMock;

        /// <summary>
        /// Mock used to configure and verify interactions with the countries service.
        /// </summary>
        private readonly Mock<ICountriesService> _countriesServiceMock;

        /// <summary>
        /// Mock logger for the persons controller.
        /// </summary>
        private readonly Mock<ILogger<PersonsController>> _loggerMock;

        /// <summary>
        /// Provides the fixture used by the test class.
        /// </summary>
        private readonly Fixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsControllerTestISP"/> class with a fixture and mocked
        /// service dependencies.
        /// </summary>
        public PersonsControllerTestISP()
        {
            this._fixture = new Fixture();

            this._countriesServiceMock = new Mock<ICountriesService>();
            this._personsGetterServiceMock = new Mock<IPersonsGetterService>();
            this._personsAdderServiceMock = new Mock<IPersonsAdderService>();
            this._personsUpdaterServiceMock = new Mock<IPersonsUpdaterService>();
            this._personsDeleterServiceMock = new Mock<IPersonsDeleterService>();
            this._personsSorterServiceMock = new Mock<IPersonsSorterService>();
            this._loggerMock = new Mock<ILogger<PersonsController>>();

            this._countriesService = this._countriesServiceMock.Object;
            this._personsGetterService = this._personsGetterServiceMock.Object;
            this._personsAdderService = this._personsAdderServiceMock.Object;
            this._personsUpdaterService = this._personsUpdaterServiceMock.Object;
            this._personsDeleterService = this._personsDeleterServiceMock.Object;
            this._personsSorterService = this._personsSorterServiceMock.Object;
            this._logger = this._loggerMock.Object;
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

            var personsController = new PersonsController(this._personsGetterService, this._personsAdderService, this._personsUpdaterService, this._personsDeleterService, this._personsSorterService, this._countriesService, this._logger);

            this._personsGetterServiceMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(persons_response_list);

            this._personsSorterServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(persons_response_list);

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

            var personsController = new PersonsController(this._personsGetterService, this._personsAdderService, this._personsUpdaterService, this._personsDeleterService, this._personsSorterService, this._countriesService, this._logger);

            this._countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);
            this._personsAdderServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

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

            var personsController = new PersonsController(this._personsGetterService, this._personsAdderService, this._personsUpdaterService, this._personsDeleterService, this._personsSorterService, this._countriesService, this._logger);

            this._countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);
            this._personsAdderServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

            // Act
            var result = await personsController.Create(persons_add_request);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}
