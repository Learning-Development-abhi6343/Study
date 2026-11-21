// <copyright file="CustomControllerBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// 
/// </summary>
// [Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]

// [Route("api/[controller]")]
[ApiController]
public class CustomControllerBase : ControllerBase
{
}