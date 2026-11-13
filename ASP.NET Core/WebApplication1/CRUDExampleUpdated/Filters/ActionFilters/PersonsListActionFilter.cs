// <copyright file="PersonsListActionFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Filters.ActionFilters
{
    using CRUDExampleUpdated.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using ServiceContractsxUnit.DTO;
    using ServiceContractsxUnit.Enums;

    /// <summary>
    /// Represents an action filter for handling actions related to the list of persons.
    /// </summary>
    public class PersonsListActionFilter : IActionFilter
    {
        /// <summary>
        /// Gets the logger instance for logging information related to the action filter.
        /// </summary>
        readonly ILogger<PersonsListActionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsListActionFilter"/> class with the specified logger.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Called after the action method is executed. Logs information about the execution of the action.
        /// </summary>
        /// <param name="context">The action executed context.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // this._logger.LogInformation("OnActionExecuted method of PersonsListActionFilter");
            this._logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(this.OnActionExecuted));
            var personController = context.Controller as PersonsController;
            var viewData = personController?.ViewData;

            var parameters = context.HttpContext.Items["arguments"] as IDictionary<string, object>;
            if (parameters?.ContainsKey("searchBy") == true)
            {
                viewData?["CurrentSearchBy"] = Convert.ToString(parameters["searchBy"]);
            }

            if (parameters?.ContainsKey("searchString") == true)
            {
                viewData?["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
            }

            if (parameters?.ContainsKey("sortBy") == true)
            {
                viewData?["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
            }
            else
            {
                viewData?["CurrentSortBy"] = nameof(PersonResponse.PersonName);
            }

            if (parameters?.ContainsKey("sortOrder") == true)
            {
                var sortOrder = parameters["sortOrder"];
                if (sortOrder is SortOrderOptions sortOrderEnum)
                {
                    viewData?["CurrentSortOrder"] = sortOrderEnum.ToString();
                }
                else
                {
                    viewData?["CurrentSortOrder"] = Convert.ToString(sortOrder);
                }
            }
            else
            {
                viewData?["CurrentSortOrder"] = SortOrderOptions.ASC.ToString();
            }

            personController?.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Age), "Age" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.Country), "Country" },
                { nameof(PersonResponse.Address), "Address" },
                { nameof(PersonResponse.ReceiveNewsLetters), "Receive News Letters" },
            };
        }

        /// <summary>
        /// Called before the action method is executed. Logs information about the execution of the action.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;

            // To do: add before logic here
            // this._logger.LogInformation("OnActionExecuting method of PersonsListActionFilter");
            this._logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(this.OnActionExecuting));

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                var searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                // Check if the searchBy parameter is not null or empty
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address),
                    };

                    if (searchByOptions.Any(temp => temp == searchBy))
                    {
                        this._logger.LogInformation("SearchBy parameter value: {searchBy}", searchBy);
                    }

                    // Reset the searchBy parameter to PersonName if it is not a valid option
                    else
                    {
                        this._logger.LogInformation("SearchBy parameter actual value: {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        this._logger.LogInformation("SearchBy parameter updated value: {searchBy}", searchBy);
                    }
                }
            }
        }
    }
}
