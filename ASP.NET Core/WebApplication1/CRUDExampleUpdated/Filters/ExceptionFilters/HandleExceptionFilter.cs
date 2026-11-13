// <copyright file="HandleExceptionFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Filters.ExceptionFilters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// An exception filter that handles exceptions thrown during the execution of an action method.
    /// </summary>
    public class HandleExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// The logger instance used to log exception details.
        /// </summary>
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionFilter"/> class with the specified logger.
        /// </summary>
        /// <param name="logger">The logger instance used to log exception details.</param>
        /// <param name="hostEnvironment">The host environment instance.</param>
        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment)
        {
            this._logger = logger;
            this._hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Called when an exception occurs during the execution of an action method. Logs the exception details using the provided logger.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public void OnException(ExceptionContext context)
        {
            // this._logger.LogError(context.Exception, "An error occurred while processing the request.");
            this._logger.LogError("Exception filter {FilterName}.{MehodName}\n{ExceptionType}\n{ExceptionMessage}", nameof(HandleExceptionFilter), nameof(this.OnException), context.Exception.GetType().Name, context.Exception.Message);

            if (this._hostEnvironment.IsDevelopment())
            {
                context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };
            }
        }
    }
}
