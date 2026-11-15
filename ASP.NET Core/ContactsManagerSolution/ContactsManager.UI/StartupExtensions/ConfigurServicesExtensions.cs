// <copyright file="ConfigurServicesExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.UI.StartupExtensions
{
    using ContactsManager.Core.Domain.IdentityEntities;
    using ContactsManager.Core.Domain.RepositoryContracts;
    using ContactsManager.Core.ServiceContracts;
    using ContactsManager.Core.Services;
    using ContactsManager.Infrastructure.DbContexts;
    using ContactsManager.Infrastructure.Repositories;
    using ContactsManager.UI.Filters.ActionFilters;
    using Microsoft.AspNetCore.HttpLogging;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

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
            services.AddControllersWithViews(options =>
            {
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                options.Filters.Add(new ResponseHeaderActionFilter(logger) { _key = "MyKey-From-Global", _value = "MyValue-From-Global", Order = 2 });
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Enable Identity in this project
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDbIdentityContext>().AddDefaultTokenProviders().AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbIdentityContext, Guid>>().AddRoleStore<RoleStore<ApplicationRole, ApplicationDbIdentityContext, Guid>>();

            // Add services into IoC container
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<ICountriesGetterService, CountriesGetterService>();
            services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();
            services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>();
            services.AddScoped<PersonsGetterService, PersonsGetterService>();
            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();

            services.AddTransient<PersonsListActionFilter>();
            services.AddTransient<ResponseHeaderActionFilter>();
            services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders);

            return services;
        }
    }
}
