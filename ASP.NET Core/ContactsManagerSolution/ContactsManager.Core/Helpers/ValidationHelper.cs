// <copyright file="ValidationHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.Helpers
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides utilities for validating model objects.
    /// </summary>
    /// <remarks>Model validation uses data annotation attributes. Invalid models cause an <see
    /// cref="ArgumentException"/>.</remarks>
    public class ValidationHelper
    {
        /// <summary>
        /// Validates an object against its validation attributes.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <exception cref="ArgumentException">The object fails validation.</exception>
        public static void ModelValidation(object obj)
        {
            var validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = [];
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
