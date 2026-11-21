// <copyright file="CitiesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CitiesManager.WebAPI.Controllers.v2
{
    using Asp.Versioning;
    using CitiesManager.WebAPI.DatabaseContext;
    using CitiesManager.WebAPI.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("2.0")]
    public class CitiesController : CustomControllerBase

    // public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CitiesController(ApplicationDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// To get list of cities (including city ID and city name) from 'cities' table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string?>>> GetCities()
        {
            return await this._context.Cities.OrderBy(temp => temp.CityName).Select(temp => temp.CityName).ToListAsync();
        }
    }
}