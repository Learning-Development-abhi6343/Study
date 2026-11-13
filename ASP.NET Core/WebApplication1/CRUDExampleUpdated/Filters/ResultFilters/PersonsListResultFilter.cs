// <copyright file="PersonsListResultFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Filters.ResultFilters
{
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Represents a result filter that performs pre- and post-processing of the action result for the "PersonsList" action.
    /// </summary>
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        /// <summary>
        /// Gets the logger instance for logging information related to the result filter.
        /// </summary>
        private readonly ILogger<PersonsListResultFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsListResultFilter"/> class with the specified logger.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Executes the result filter asynchronously, allowing for pre- and post-processing of the action result.
        /// </summary>
        /// <param name="context">The result executing context.</param>
        /// <param name="next">The result execution delegate.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // TO DO: before the action result executes
            this._logger.LogInformation("{FilterName}.{MethodName} - before called", nameof(PersonsListResultFilter), nameof(this.OnResultExecutionAsync));
            await next(); // Call the next result filter or action result in the pipeline

            // TO DO: after the action result executes
            this._logger.LogInformation("{FilterName}.{MethodName} - after called", nameof(PersonsListResultFilter), nameof(this.OnResultExecutionAsync));

            if (!context.HttpContext.Response.HasStarted)
            {
                context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                this._logger.LogWarning("Cannot set Last-Modified because response has already started.");
            }
        }
    }
}