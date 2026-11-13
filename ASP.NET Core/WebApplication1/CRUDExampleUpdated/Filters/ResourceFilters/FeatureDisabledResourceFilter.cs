// <copyright file="FeatureDisabledResourceFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Filters.ResourceFilters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Represents a resource filter that checks if a feature is disabled and returns a 404 Not Found result if it is. Otherwise, it allows the action method to execute.
    /// </summary>
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        /// <summary>
        /// Gets the logger instance for logging information related to the resource filter.
        /// </summary>
        private readonly ILogger<FeatureDisabledResourceFilter> _logger;

        /// <summary>
        /// Gets a value indicating whether the feature is disabled. If true, the filter will return a 404 Not Found result; otherwise, it will allow the action method to execute.
        /// </summary>
        private readonly bool _isDisabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureDisabledResourceFilter"/> class with the specified logger and feature disabled status.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="isDisabled">A value indicating whether the feature is disabled.</param>
        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled)
        {
            this._logger = logger;
            this._isDisabled = isDisabled;
        }

        /// <summary>
        /// This method is called before and after the action method executes. It checks if the feature is disabled and returns a 404 Not Found result if it is. Otherwise, it allows the action method to execute.
        /// </summary>
        /// <param name="context">The resource executing context.</param>
        /// <param name="next">The resource execution delegate.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (this._isDisabled)
            {
                // context.Result = new NotFoundResult(); // 404 Not Found
                context.Result = new StatusCodeResult(501); // 501 Not Implemented
                return;
            }

            // TO DO: before the action method executes
            this._logger.LogInformation("{FilterName}.{MethodName} - before called", nameof(FeatureDisabledResourceFilter), nameof(this.OnResourceExecutionAsync));
            await next();

            // TO DO: after the action method executes
            this._logger.LogInformation("{FilterName}.{MethodName} - after called", nameof(FeatureDisabledResourceFilter), nameof(this.OnResourceExecutionAsync));
        }
    }
}
