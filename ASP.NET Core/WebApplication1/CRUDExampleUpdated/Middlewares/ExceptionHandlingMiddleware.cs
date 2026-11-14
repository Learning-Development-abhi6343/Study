// <copyright file="ExceptionHandlingMiddleware.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Middlewares
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Serilog;

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project

    /// <summary>
    /// Middleware that invokes the next component in the HTTP request pipeline and catches exceptions raised during
    /// request processing.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        /// <summary>
        /// The next middleware component in the request pipeline.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Provides logging for exception handling middleware.
        /// </summary>
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Provides diagnostic context for enriching request completion events.
        /// </summary>
        private readonly IDiagnosticContext _diagnosticContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="logger">The logger used to record exception-related information.</param>
        /// <param name="diagnosticContext">The diagnostic context used to enrich request diagnostic events.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IDiagnosticContext diagnosticContext)
        {
            this._next = next; // Represents the subsequent middleware in the HTTP request pipeline.
            this._logger = logger;
            this._diagnosticContext = diagnosticContext;
        }

        /// <summary>
        /// Calls the next middleware and handles exceptions by logging details and writing a generic error message to the response.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the current request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this._next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    this._logger.LogError("{ExceptionType} {ExceptionMessage}", ex.InnerException.GetType().Name, ex.InnerException.Message);
                }
                else
                {
                    this._logger.LogError("{ExceptionType} {ExceptionMessage}", ex.GetType().Name, ex.Message);
                }

                /*httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsync("An unexpected error occurred. Please try again later.");*/

                throw; // Re-throw the exception to allow further handling by other middleware or the framework.
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.

    /// <summary>
    /// Provides extension methods for adding middleware to an application's HTTP request pipeline.
    /// </summary>
    public static class ExceptionHandlingMiddlewareExtensions
    {
        /// <summary>
        /// Adds exception handling middleware to the application pipeline.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The application builder, configured with the exception handling middleware.</returns>
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
