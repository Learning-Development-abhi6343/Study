// <copyright file="SkipFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExampleUpdated.Filters
{
    /// <summary>
    /// Indicates that a filter should be skipped.
    /// </summary>
    public class SkipFilter : Attribute, IFilterMetadata
    {
    }
}
