// <copyright file="ApplicationUser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.Domain.IdentityEntities
{
    using System;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Represents an application user in the identity system, extending the IdentityUser class with a unique identifier of type Guid and an optional PersonName property.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets or sets the name of the person associated with this application user.
        /// </summary>
        public string? PersonName { get; set; }
    }
}
