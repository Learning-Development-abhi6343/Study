// <copyright file="PersonAlwaysRunResultFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Filters.ResultFilters
{
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// A result filter that always runs, regardless of the outcome of the action method. This filter can be used to perform actions before and after the result is executed, such as logging or modifying the response.
    /// </summary>
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        /// <summary>
        /// Called after the action result has been executed. This method can be used to perform actions after the result is executed, such as logging or modifying the response.
        /// </summary>
        /// <param name="context">The result executed context.</param>
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        /// <summary>
        /// Called before the action result is executed. This method can be used to perform actions before the result is executed, such as logging or modifying the response.
        /// </summary>
        /// <param name="context">The result executing context.</param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            // if (context.Filters.OfType<SkipFilter>().Any())
            if (context.Filters.Any(f => f is SkipFilter)) // Check if the SkipFilter is applied to the action or controller
            {
                return;
            }

            // TO DO: before the result executes
        }
    }
}
