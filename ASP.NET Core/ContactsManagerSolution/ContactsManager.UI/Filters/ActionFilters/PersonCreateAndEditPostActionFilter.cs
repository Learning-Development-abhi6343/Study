// <copyright file="PersonCreateAndEditPostActionFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.UI.Filters.ActionFilters
{
    using ContactsManager.Core.ServiceContracts;
    using ContactsManager.UI.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// An action filter that handles the validation of the model state for the Create and Edit actions in the PersonsController.
    /// </summary>
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Provides access to the service for retrieving country information.
        /// </summary>
        private readonly ICountriesGetterService _countriesGetterService;

        /// <summary>
        /// The logger instance used to log information related to the execution of the action filter.
        /// </summary>
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;

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