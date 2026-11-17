// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using ContactsManager.UI.Middlewares;
using ContactsManager.UI.StartupExtensions;
using Rotativa.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);

// Serilog
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration) // Read configuration from appsettings.json
    .ReadFrom.Services(services); // Read configuration from DI services
});

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();

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
app.UseRouting();   // Identifying action method to be executed based on the request URL
app.UseAuthentication();    // Add authentication middleware to the request pipeline // Reading Identity from cookie and setting it to HttpContext.User
app.UseAuthorization();    // Add authorization middleware to the request pipeline // Checking user permissions for the requested resource
app.MapControllers();   // Execute the filter pipeline for the identified action method and execute the action method

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}");  // Admin/Home/Index, Admin
    endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action}/{id?}");
});
app.Run();