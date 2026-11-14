// <copyright file="PersonsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Controllers
{
    using CRUDExampleUpdated.Filters;
    using CRUDExampleUpdated.Filters.ActionFilters;
    using CRUDExampleUpdated.Filters.AuthorizationFilters;
    using CRUDExampleUpdated.Filters.ExceptionFilters;
    using CRUDExampleUpdated.Filters.ResourceFilters;
    using CRUDExampleUpdated.Filters.ResultFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Rotativa.AspNetCore;
    using Rotativa.AspNetCore.Options;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;
    using ServiceContractsxUnit.Enums;

    /// <summary>
    /// Represents a controller for managing persons and their related actions.
    /// </summary>
    [Route("[controller]")]
    [ResponseHeaderFilterFactory("MyKey-From-Controller", "MyValue-From-Controller", 3)]

    // [TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        /// <summary>
        /// The service for managing persons.
        /// </summary>

        // readonly IPersonsService _personsService;

        /// <summary>
        /// The service for retrieving persons.
        /// </summary>
        private readonly IPersonsGetterService _personsGetterService;

        /// <summary>
        /// Service for adding persons.
        /// </summary>
        private readonly IPersonsAdderService _personsAdderService;

        /// <summary>
        /// Provides access to the service for updating person records.
        /// </summary>
        private readonly IPersonsUpdaterService _personsUpdaterService;

        /// <summary>
        /// The service used to delete persons.
        /// </summary>
        private readonly IPersonsDeleterService _personsDeleterService;

        /// <summary>
        /// Service used to sort persons.
        /// </summary>
        private readonly IPersonsSorterService _personsSorterService;

        /// <summary>
        /// The service for managing countries.
        /// </summary>
        readonly ICountriesService _countriesService;

        /// <summary>
        /// The logger for logging information related to the controller's actions.
        /// </summary>
        readonly ILogger<PersonsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsController"/> class.
        /// </summary>
        /// <param name="personsGetterService">The service for retrieving persons.</param>
        /// <param name="personsAdderService">The service for adding persons.</param>
        /// <param name="personsUpdaterService">The service for updating persons.</param>
        /// <param name="personsDeleterService">The service for deleting persons.</param>
        /// <param name="personsSorterService">The service for sorting persons.</param>
        /// <param name="countriesService">The service for retrieving country information.</param>
        /// <param name="logger">The logger for recording controller activity.</param>
        public PersonsController(IPersonsGetterService personsGetterService, IPersonsAdderService personsAdderService, IPersonsUpdaterService personsUpdaterService, IPersonsDeleterService personsDeleterService, IPersonsSorterService personsSorterService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            this._personsGetterService = personsGetterService;
            this._personsAdderService = personsAdderService;
            this._personsUpdaterService = personsUpdaterService;
            this._personsDeleterService = personsDeleterService;
            this._personsSorterService = personsSorterService;
            this._countriesService = countriesService;
            this._logger = logger;
        }

        /// <summary>
        /// Handles the request to display a list of persons with optional search and sorting parameters.
        /// </summary>
        /// <param name="searchBy">The field to search by.</param>
        /// <param name="searchString">The string to search for.</param>
        /// <param name="sortBy">The field to sort by.</param>
        /// <param name="sortOrder">The order to sort in.</param>
        /// <returns>The action result.</returns>
        [Route("[action]")]
        [Route("/")]
        [ResponseHeaderFilterFactory("X-Custom_Key", "Custom-Value", 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [ServiceFilter(typeof(PersonsListActionFilter))]
        [SkipFilter]

        public async Task<IActionResult> Index(string searchBy, string searchString, string? sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            this._logger.LogInformation("Index action method of PersonsController");

            this._logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");

            var persons = await this._personsGetterService.GetFilteredPersons(searchBy, searchString);

            var sortedPersons = await this._personsSorterService.GetSortedPersons(persons, sortBy!, sortOrder);

            return this.View(sortedPersons);
        }

        /// <summary>
        /// Handles the request to display the form for creating a new person.
        /// </summary>
        /// <returns>The action result.</returns>
        // Executes when user clicks on "Create Person" link
        [Route("create")]
        [HttpGet]
        [ResponseHeaderFilterFactory("my-key", "my-value", 4)]
        public async Task<IActionResult> Create()
        {
            var countries = await this._countriesService.GetAllCountries();
            this.ViewBag.Countries = countries.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });
            return this.View();
        }

        /// <summary>
        /// Handles the request to create a new person based on the provided person add request.
        /// </summary>
        /// <param name="personRequest">The person add request.</param>
        /// <returns>The action result.</returns>
        [Route("create")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] { false })]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            await this._personsAdderService.AddPerson(personRequest);

            // navigate user to "Index" action of this controller
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the form for editing an existing person.
        /// </summary>
        /// <param name="personID">The ID of the person to edit.</param>
        /// <returns>The action result.</returns>
        [Route("[action]/{personID}")]
        [HttpGet]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid personID)
        {
            var personResponse = await this._personsGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return this.RedirectToAction("Index");
            }

            var personUpdateRequest = personResponse.ToPersonUpdateRequest();

            var countries = await this._countriesService.GetAllCountries();
            this.ViewBag.Countries = countries.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });

            return this.View(personUpdateRequest);
        }

        /// <summary>
        /// Updates an existing person.
        /// </summary>
        /// <param name="personRequest">The request containing the updated person information.</param>
        /// <returns>The action result.</returns>
        [Route("[action]/{personID}")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            var personResponse = await this._personsGetterService.GetPersonByPersonID(personRequest.PersonID);
            if (personResponse == null)
            {
                return this.RedirectToAction("Index");
            }

            // personRequest.PersonID = Guid.NewGuid();
            var updatedPerson = await this._personsUpdaterService.UpdatePerson(personRequest);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the delete confirmation page for a specific person.
        /// </summary>
        /// <param name="personID">The ID of the person to delete.</param>
        /// <returns>The action result.</returns>
        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid personID)
        {
            var personResponse = await this._personsGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return this.RedirectToAction("Index");
            }

            return this.View(personResponse);
        }

        /// <summary>
        /// Deletes a specific person.
        /// </summary>
        /// <param name="personUpdateRequest">The request containing the person to delete.</param>
        /// <returns>The action result.</returns>
        [Route("[action]/{personID}")]
        [HttpPost]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            var personResponse = await this._personsGetterService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (personResponse == null)
            {
                return this.RedirectToAction("Index");
            }

            await this._personsDeleterService.DeletePerson(personUpdateRequest.PersonID);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Generates a PDF document containing the list of persons and returns it as a response.
        /// </summary>
        /// <returns>The action result.</returns>
        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            // Get list of persons
            var persons = await this._personsGetterService.GetAllPersons();

            // Return view as pdf
            return new ViewAsPdf("PersonsPDF", persons, this.ViewData)
            {
                PageMargins = new Margins(20, 10, 20, 10),
                PageOrientation = Orientation.Landscape,
            };
        }

        /// <summary>
        /// Generates a CSV file containing the list of persons and returns it as a response.
        /// </summary>
        /// <returns>The action result.</returns>
        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            var memoryStream = await this._personsGetterService.GetPersonsCSV();
            return this.File(memoryStream, "application/octet-stream", "persons.csv");
        }

        /// <summary>
        /// Generates an Excel file containing the list of persons and returns it as a response.
        /// </summary>
        /// <returns>The action result.</returns>
        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            var memoryStream = await this._personsGetterService.GetPersonsExcel();
            return this.File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
