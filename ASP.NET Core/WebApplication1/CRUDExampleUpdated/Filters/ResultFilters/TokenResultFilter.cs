// <copyright file="TokenResultFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Filters.ResultFilters
{
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Represents a result filter that appends an "Auth-Key" cookie to the HTTP response before the action result is executed.
    /// </summary>
    public class TokenResultFilter : IResultFilter
    {
        /// <summary>
        /// This method is called after the action result has been executed. It can be used to perform any post-processing or cleanup tasks related to the action result.
        /// </summary>
        /// <param name="context">The result executed context.</param>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        /// <summary>
        /// This method is called before the action result is executed. It appends an "Auth-Key" cookie with the value "A100" to the HTTP response.
        /// </summary>
        /// <param name="context">The result executing context.</param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
        }
    }
}
