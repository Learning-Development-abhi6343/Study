// <copyright file="ResponseHeaderActionFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.UI.Filters.ActionFilters
{
    using Microsoft.AspNetCore.Mvc.Filters;   

    /// <summary>
    /// Represents an action filter that adds a custom response header to the HTTP response.
    /// </summary>
    public class ResponseHeaderActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Gets the logger instance for logging information related to the action filter.
        /// </summary>
        private readonly ILogger<ResponseHeaderActionFilter> _logger;

        /// <summary>
        /// Gets the key for the custom response header.
        /// </summary>
        // private readonly string _key;
        public string _key { get; set; }

        /// <summary>
        /// Gets the value for the custom response header.
        /// </summary>
        // private readonly string _value;
        public string _value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeaderActionFilter"/> class with the specified logger, key, value, and order.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Called before and after the action method is executed to perform any pre-processing or post-processing logic.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        /// <param name="next">The delegate to the next filter in the pipeline.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
             this._logger.LogInformation("{FilterName}.{MethodName} method - before called", nameof(ResponseHeaderActionFilter), nameof(this.OnActionExecutionAsync));
             await next(); // Call the next action filter or action method in the pipeline

             this._logger.LogInformation("{FilterName}.{MethodName} method - after called", nameof(ResponseHeaderActionFilter), nameof(this.OnActionExecutionAsync));
             context.HttpContext?.Response?.Headers?[this._key] = this._value;
        }
    }

    /// <summary>
    /// Represents a factory attribute that creates instances of the <see cref="ResponseHeaderActionFilter"/> class.
    /// </summary>
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        private readonly string _key;
        private readonly string _value;
        private readonly int _order;

        /// <summary>
        /// Gets a value indicating whether the filter instance created by this factory is reusable. If true, the same instance can be used for multiple requests; otherwise, a new instance is created for each request.
        /// </summary>
        public bool IsReusable => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeaderFilterFactoryAttribute"/> class with the specified key, value, and order for the response header.
        /// </summary>
        /// <param name="key">The key for the response header.</param>
        /// <param name="value">The value for the response header.</param>
        /// <param name="order">The order of the filter in the pipeline.</param>
        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
        {
            this._key = key;
            this._value = value;
            this._order = order;
        }

        /// <summary>
        /// Creates an instance of the <see cref="ResponseHeaderActionFilter"/> class using the provided service provider. The created filter instance is configured with the specified key, value, and order.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>The created filter instance.</returns>
        // Controller -> FilterFactory -> Filter -> Action
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            // var filter = new ResponseHeaderActionFilter(this._key, this._value, this._order);
            // var filter = new ResponseHeaderActionFilter
            // {
            //     _key = this._key,
            //     _value = this._value,
            //     Order = this._order
            // };
            // var filter = ActivatorUtilities.CreateInstance<ResponseHeaderActionFilter>(serviceProvider);
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();

            filter._key = this._key;
            filter._value = this._value;
            filter.Order = this._order;
            return filter;
        }
    }
}
