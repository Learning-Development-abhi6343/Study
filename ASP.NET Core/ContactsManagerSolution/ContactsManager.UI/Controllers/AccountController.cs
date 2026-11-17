// <copyright file="AccountController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.UI.Controllers
{
    using ContactsManager.Core.Domain.IdentityEntities;
    using ContactsManager.Core.DTO;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents the controller responsible for handling user account-related actions, such as registration.
    /// </summary>
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        /// <summary>
        /// The user manager used for managing user accounts and authentication.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// The sign-in manager used for handling user sign-in and authentication.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class with the specified user manager.
        /// </summary>
        /// <param name="userManager">The user manager used for managing user accounts and authentication.</param>
        /// <param name="signInManager">The sign-in manager used for handling user sign-in and authentication.</param>
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

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
        /// Handles the user registration process by validating the input, creating a new user account, and redirecting to the appropriate page based on the result.
        /// </summary>
        /// <param name="registerDTO">The data transfer object containing the user registration details.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            // Check for validation errors
            if (!this.ModelState.IsValid)
            {
                this.ViewBag.Errors = this.ModelState.Values.SelectMany(temp => temp.Errors.Select(e => e.ErrorMessage));
                return this.View(registerDTO);
            }

            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName,
            };

            var result = await this._userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                // Sign in
                await this._signInManager.SignInAsync(user, isPersistent: false);

                // TO DO: Store user registration details into Identity database using ApplicationDbContext
                return this.RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("Register", error.Description);
                }

                return this.View(registerDTO);
            }
        }

        /// <summary>
        /// Displays the user login form.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        /// <summary>
        /// Handles the user login process by validating the input, attempting to sign in the user, and redirecting to the appropriate page based on the result.
        /// </summary>
        /// <param name="loginDTO">The data transfer object containing the user login details.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewBag.Errors = this.ModelState.Values.SelectMany(temp => temp.Errors.Select(e => e.ErrorMessage));
                return this.View(loginDTO);
            }

            var result = await this._signInManager.PasswordSignInAsync(loginDTO.Email ?? string.Empty, loginDTO.Password ?? string.Empty, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return this.RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                this.ModelState.AddModelError("Login", "Invalid email or password.");
                return this.View(loginDTO);
            }
        }

        /// <summary>
        /// Logs out the currently authenticated user and redirects to the index page of the Persons controller.
        /// </summary>
        /// <returns>The action result.</returns>
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();
            return this.RedirectToAction(nameof(PersonsController.Index), "Persons");
        }
    }
}
