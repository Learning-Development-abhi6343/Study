// <copyright file="ApplicationRole.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Core.Domain.IdentityEntities
{
    using System;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Represents an application role in the identity system, extending the IdentityRole class with a unique identifier of type Guid.
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}
