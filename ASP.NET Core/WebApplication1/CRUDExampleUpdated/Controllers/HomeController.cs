// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.Controllers
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Handles requests for the home page and error view.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the error page.
        /// </summary>
        /// <returns>The error view.</returns>
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            {
                this.ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
            }

            return this.View(); // Views/Home/Error.cshtml
        }
    }
}
