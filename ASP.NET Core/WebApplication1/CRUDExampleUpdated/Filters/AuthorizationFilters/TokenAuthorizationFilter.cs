namespace CRUDExampleUpdated.Filters.AuthorizationFilters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class TokenAuthorizationFilter : IAsyncAuthorizationFilter
    {
        /// <summary>
        /// Gets the logger instance for logging information related to the authorization filter.
        /// </summary>
        private readonly ILogger<TokenAuthorizationFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenAuthorizationFilter"/> class with the specified logger.
        /// </summary>
        /// <param name="logger">The logger instance for logging information related to the authorization filter.</param>
        public TokenAuthorizationFilter(ILogger<TokenAuthorizationFilter> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// This method is called to authorize the request. It checks for the presence of an "Auth-Key" cookie and validates its value. If the cookie is missing or has an invalid value, it sets the result to a 401 Unauthorized response.
        /// </summary>
        /// <param name="context">The authorization filter context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Cookies.ContainsKey("Auth-Key"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized); // new UnauthorizedResult();
                return;
            }

            if (context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
        }
    }
}
