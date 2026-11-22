// <copyright file="CustomEndPointFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MinimalAPI.EndPointFilters
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomEndPointFilter : IEndpointFilter
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<CustomEndPointFilter> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CustomEndPointFilter(ILogger<CustomEndPointFilter> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // Before logic
            this._logger.LogInformation("Endppoint filter - before logic");
            var result = await next(context); // It invokes the subsequent filter or endpoint's request delegate

            // After logic
            this._logger.LogInformation("Endppoint filter - after logic");
            return result;
        }
    }
}
