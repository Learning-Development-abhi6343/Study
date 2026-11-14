// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CRUDExampleUpdated.Filters.ActionFilters;
using CRUDExampleUpdated.StartupExtensions;
using CRUDExampleUpdated.Middlewares;
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

var app = builder.Build();

app.UseSerilogRequestLogging(); // Add Serilog request logging middleware
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

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