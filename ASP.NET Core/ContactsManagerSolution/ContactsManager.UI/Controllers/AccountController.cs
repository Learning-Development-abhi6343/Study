// <copyright file="AccountController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.UI.Controllers
{
    using ContactsManager.Core.DTO;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents the controller responsible for handling user account-related actions, such as registration.
    /// </summary>
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        /// <summary>
        /// Displays the user registration form.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        /// <summary>
        /// Handles the user registration form submission and stores the user registration details into the Identity database using ApplicationDbIdentityContext.
        /// </summary>
        /// <param name="registerDTO">The data transfer object containing the user registration details.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public IActionResult Register(RegisterDTO registerDTO)
        {
            // TO DO: Store user registration details into Identity database using ApplicationDbIdentityContext
            return this.RedirectToAction(nameof(PersonsController.Index), "Persons");
        }
    }
}
