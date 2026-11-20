// <copyright file="CitiesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//[Route("api/[controller]")]
//[ApiController]
public class CitiesController : CustomControllerBase

// public class CitiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CitiesController(ApplicationDbContext context)
    {
        this._context = context;
    }

    // GET: api/City
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCity()
    {
        return await this._context.Cities.ToListAsync();
    }

    // GET: api/City/5
    [HttpGet("{cityid}")]
    public async Task<ActionResult<City>> GetCity(Guid cityid)
    {
        var city = await this._context.Cities.FindAsync(cityid);

        if (city == null)
        {
            return this.Problem(detail: "Invalid CityID", statusCode: 400, title: "City Search");

             // return this.NotFound();
        }

        return city;
    }

    // PUT: api/City/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{cityid}")]

    // public async Task<IActionResult> PutCity(Guid? cityid, City city)
    public async Task<IActionResult> PutCity(Guid? cityid, [Bind(nameof(City.CityID), nameof(City.CityName))] City city)
    {
        if (cityid != city.CityID)
        {
            return this.BadRequest();
        }

        // this._context.Entry(city).State = EntityState.Modified;
        var existingCity = await this._context.Cities.FindAsync(cityid.Value);
        if (existingCity == null)
        {
            return this.NotFound();
        }

        existingCity.CityName = city.CityName;
        try
        {
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.CityExists(cityid))
            {
                return this.NotFound();
            }
            else
            {
                throw;
            }
        }

        return this.NoContent();
    }

    // POST: api/City
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<City>> PostCity([Bind(nameof(City.CityID), nameof(City.CityName))] City city)
    {
        this._context.Cities.Add(city);
        await this._context.SaveChangesAsync();

        return this.CreatedAtAction("GetCity", new { cityid = city.CityID }, city);
    }

    // DELETE: api/City/5
    [HttpDelete("{cityid}")]
    public async Task<IActionResult> DeleteCity(System.Guid? cityid)
    {
        var city = await _context.Cities.FindAsync(cityid);
        if (city == null)
        {
            return NotFound();
        }

        this._context.Cities.Remove(city);
        await this._context.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(System.Guid? cityid)
    {
        return _context.Cities.Any(e => e.CityID == cityid);
    }
}
