// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CRUDExampleUpdated.Filters.ActionFilters;
using CRUDExampleUpdated.StartupExtensions;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repositories;
using RepositoryContracts;
using Rotativa.AspNetCore;
using Serilog;
using ServiceContractsxUnit;
using ServicesxUnit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);

// Serilog
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration) // Read configuration from appsettings.json
    .ReadFrom.Services(services); // Read configuration from DI services
});

// Logging
/* builder.Host.ConfigureLogging(loggingProvider =>
{
    loggingProvider.ClearProviders();
    loggingProvider.AddConsole();
    loggingProvider.AddDebug();
}); */

// builder.Logging.ClearProviders().AddConsole().AddDebug().AddEventLog();

/*builder.Services.AddHttpLogging(options =>
{
    // optional: configure fields, headers, etc.
    // options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
});*/

// builder.Services.AddControllersWithViews();
/*builder.Services.AddControllersWithViews(options =>
{
    // options.Filters.Add<ResponseHeaderActionFilter>(5);
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

    // options.Filters.Add(new ResponseHeaderActionFilter(logger, "MyKey-From-Global", "MyValue-From-Global"));
    // options.Filters.Add(new ResponseHeaderActionFilter(logger, "MyKey-From-Global", "MyValue-From-Global", 2));
    // options.Filters.Add(new ResponseHeaderActionFilter("MyKey-From-Global", "MyValue-From-Global", 2));
    options.Filters.Add(new ResponseHeaderActionFilter(logger) { _key = "MyKey-From-Global", _value = "MyValue-From-Global", Order = 2 });

    // options.Filters.Add<ResponseHeaderActionFilter>();
//})*/;

/* builder.Services.AddDbContext<PersonsDbContext>(options =>
    //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
); */

/*builder.Services.AddDbContext<ApplicationDbContext>(options =>

    // options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));*/

/* Add services into IoC container
builder.Services.AddSingleton<ICountriesService, CountriesService>();
builder.Services.AddSingleton<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddTransient<PersonsListActionFilter>();
builder.Services.AddTransient<ResponseHeaderActionFilter>();
builder.Services.AddScoped<ICountriesService>(sp =>
{
    var db = sp.GetRequiredService<PersonsDbContext>();
    return new CountriesService(db);
}); */

/* Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30
builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders);*/
var app = builder.Build();

app.UseSerilogRequestLogging(); // Add Serilog request logging middleware
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

/* app.Logger.LogDebug("debug-message");
app.Logger.LogInformation("information-message");
app.Logger.LogWarning("warning-message");
app.Logger.LogError("error-message");
app.Logger.LogCritical("critical-message"); */

app.UseHttpLogging();
if (!builder.Environment.IsEnvironment("Test"))
{
    RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

/// <summary>
/// Provides the application entry point and supports access from the integration test project.
/// </summary>
public partial class Program { } // Make Program class partial so that it can be accessed from integration test project