// <copyright file="ApplicationDbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ContactsManager.Infrastructure.DbContexts
{
    using System.Text.Json;
    using ContactsManager.Core.Domain.Entities;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides database access and model configuration for countries and persons.
    /// </summary>
    /// <remarks>Maps the country and person entities to their tables, seeds them from JSON files, and
    /// configures the person tax identification number. Also provides access to stored procedures for retrieving and
    /// inserting persons.</remarks>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options used to configure the context.</param>
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the set of countries.
        /// </summary>
        public virtual DbSet<Country> Countries { get; set; }

        /// <summary>
        /// Gets or sets the set of <see cref="Person"/> entities.
        /// </summary>
        public virtual DbSet<Person> Persons { get; set; }

        /// <summary>
        /// Configures the model for the context, including table mappings, seeding data from JSON files, and configuring the person tax identification number.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            // Seed to Countries
            var countriesJson = File.ReadAllText("countries.json");
            var countries = JsonSerializer.Deserialize<IEnumerable<Country>>(countriesJson);
            foreach (var country in countries ?? [])
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            // Seed to Persons
            var personsJson = File.ReadAllText("persons.json");
            var persons = JsonSerializer.Deserialize<IEnumerable<Person>>(personsJson);
            foreach (var person in persons ?? [])
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            // Fluent API
            modelBuilder.Entity<Person>().Property(p => p.TIN).HasColumnName("TaxIdentificationNumber").HasColumnType("varchar(8)").HasDefaultValue("ABC12345");

            modelBuilder.Entity<Person>().ToTable(t => t.HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8"));
        }

        /// <summary>
        /// Executes the GetAllPersons stored procedure and returns the result.
        /// </summary>
        /// <returns>An IQueryable of Person objects.</returns>
        public List<Person> sp_GetAllPersons()
        {
            return [.. this.Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]")];
        }

        /// <summary>
        /// Inserts a person record using the <c>dbo.InsertPerson</c> stored procedure.
        /// </summary>
        /// <param name="person">The person record to insert.</param>
        /// <returns>The number of rows affected.</returns>
        public int sp_InsertPerson(Person person)
        {
            var parameters = new SqlParameter[]
            {
                new("@PersonID", person.PersonID),
                new("@CountryID", person.CountryID),
                new("@PersonName", person.PersonName),
                new("@Email", person.Email),
                new("@Gender", person.Gender),
                new("@DateOfBirth", person.DateOfBirth),
                new("@Address", person.Address),
                new("@ReceiveNewsLetters", person.ReceiveNewsLetters),
            };

            return this.Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @CountryID, @PersonName, @Email, @Gender, @DateOfBirth, @Address, @ReceiveNewsLetters", parameters);
        }
    }
}
