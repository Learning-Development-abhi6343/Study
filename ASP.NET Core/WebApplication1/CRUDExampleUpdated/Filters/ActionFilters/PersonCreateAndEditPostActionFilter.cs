// <copyright file="PersonCreateAndEditPostActionFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Filters.ActionFilters
{
    using CRUDExampleUpdated.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ServiceContractsxUnit;
    using ServiceContractsxUnit.DTO;

    /// <summary>
    /// An action filter that handles the validation of the model state for the Create and Edit actions in the PersonsController.
    /// </summary>
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        /// <summary>
        /// The service used to retrieve country data for populating the country dropdown in the Create and Edit views.
        /// </summary>
        private readonly ICountriesService _countriesService;

        /// <summary>
        /// The logger instance used to log information related to the execution of the action filter.
        /// </summary>
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;

        public PersonCreateAndEditPostActionFilter(ICountriesService countriesService, ILogger<PersonCreateAndEditPostActionFilter> logger)
        {
            this._countriesService = countriesService;
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
                // TO DO: before the action executes
                if (!personsController.ModelState.IsValid)
                {
                    var countries = await this._countriesService.GetAllCountries();
                    personsController.ViewBag.Countries = countries.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });
                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage);

                    // var personAddRequest = context.ActionArguments["personAddRequest"];
                    var personAddRequest = context.ActionArguments["personRequest"];
                    context.Result = personsController.View(personAddRequest); // Short-circuit the filter and action execution and return the view with validation errors

                    // Short-circuit the filter and action execution if the model state is invalid
                    // It doesn't short-circuit result filters, so they will still execute after this filter
                    // await next(); // Call the next action filter or action method in the pipeline

                    // TO DO: after the action executes
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

            // TO DO: after the action executes
            this._logger.LogInformation("Action filter {FilterName}.{MehodName} executed successfully", nameof(PersonCreateAndEditPostActionFilter), nameof(this.OnActionExecutionAsync));
        }
    }
}