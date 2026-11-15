// <copyright file="PersonCreateAndEditPostActionFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Filters.ActionFilters
{
    using CRUDExampleUpdated.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ServiceContractsxUnit;

    /// <summary>
    /// An action filter that handles the validation of the model state for the Create and Edit actions in the PersonsController.
    /// </summary>
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Gets the service used to retrieve countries.
        /// </summary>
        private readonly ICountriesGetterService _countriesGetterService;

        /// <summary>
        /// The logger instance used to log information related to the execution of the action filter.
        /// </summary>
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonCreateAndEditPostActionFilter"/> class.
        /// </summary>
        /// <param name="countriesGetterService">The service used to retrieve countries.</param>
        /// <param name="logger">The logger used to log filter activity.</param>
        public PersonCreateAndEditPostActionFilter(ICountriesGetterService countriesGetterService, ILogger<PersonCreateAndEditPostActionFilter> logger)
        {
            this._countriesGetterService = countriesGetterService;
            this._logger = logger;
        }

        /// <summary>
        /// Called before and after the execution of an action method. Validates the model state and short-circuits the action execution if the model state is invalid, returning the appropriate view with validation errors.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        /// <param name="next">The action execution delegate.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    var countries = await this._countriesGetterService.GetAllCountries();
                    personsController.ViewBag.Countries = countries.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });
                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage);

                    var personAddRequest = context.ActionArguments["personRequest"];
                    context.Result = personsController.View(personAddRequest);
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }

            this._logger.LogInformation("Action filter {FilterName}.{MehodName} executed successfully", nameof(PersonCreateAndEditPostActionFilter), nameof(this.OnActionExecutionAsync));
        }
    }
}