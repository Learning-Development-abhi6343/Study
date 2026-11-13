// <copyright file="ConfigurServicesExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDExampleUpdated.StartupExtensions
{
    using CRUDExampleUpdated.Filters.ActionFilters;
    using Entities;
    using Microsoft.AspNetCore.HttpLogging;
    using Microsoft.EntityFrameworkCore;
    using Repositories;
    using RepositoryContracts;
    using ServiceContractsxUnit;
    using ServicesxUnit;

    /// <summary>
    /// Provides extension methods for configuring services in the application.
    /// </summary>
    public static class ConfigurServicesExtensions
    {
        /// <summary>
        /// Registers MVC, database, repository, application, action filter, and HTTP logging services.
        /// </summary>
        /// <param name="services">The service collection to which the application services are added.</param>
        /// <param name="configuration">The application configuration containing the `DefaultConnection` connection string.</param>
        /// <returns>The service collection with the application services registered.</returns>
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Logging
            /* builder.Host.ConfigureLogging(loggingProvider =>
            {
                loggingProvider.ClearProviders();
                loggingProvider.AddConsole();
                loggingProvider.AddDebug();
            }); */

            // builder.Logging.ClearProviders().AddConsole().AddDebug().AddEventLog();

            /*services.AddHttpLogging(options =>
            {
                // optional: configure fields, headers, etc.
                // options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
            });*/

            // services.AddControllersWithViews();
            services.AddControllersWithViews(options =>
            {
                // options.Filters.Add<ResponseHeaderActionFilter>(5);
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

                // options.Filters.Add(new ResponseHeaderActionFilter(logger, "MyKey-From-Global", "MyValue-From-Global"));
                // options.Filters.Add(new ResponseHeaderActionFilter(logger, "MyKey-From-Global", "MyValue-From-Global", 2));
                // options.Filters.Add(new ResponseHeaderActionFilter("MyKey-From-Global", "MyValue-From-Global", 2));
                options.Filters.Add(new ResponseHeaderActionFilter(logger) { _key = "MyKey-From-Global", _value = "MyValue-From-Global", Order = 2 });

                // options.Filters.Add<ResponseHeaderActionFilter>();
            });

            /* services.AddDbContext<PersonsDbContext>(options =>
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            ); */

            services.AddDbContext<ApplicationDbContext>(options =>

                // options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add services into IoC container
            // services.AddSingleton<ICountriesService, CountriesService>();
            // services.AddSingleton<IPersonsService, PersonsService>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonsService, PersonsService>();
            services.AddTransient<PersonsListActionFilter>();
            services.AddTransient<ResponseHeaderActionFilter>();
            /* services.AddScoped<ICountriesService>(sp =>
            {
                var db = sp.GetRequiredService<PersonsDbContext>();
                return new CountriesService(db);
            }); */

            // Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30
            services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders);

            return services;
        }
    }
}
