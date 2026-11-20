using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CitiesManager.WebAPI.Migrations;

/// <inheritdoc />
public partial class _20261117173929_Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Cities",
            columns: table => new
            {
                CityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CityName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cities", x => x.CityID);
            });

        migrationBuilder.InsertData(
            table: "Cities",
            columns: new[] { "CityID", "CityName" },
            values: new object[,]
            {
                { new Guid("ccec09c0-2c05-4610-bf22-fbaaeb94d0ec"), "New York" },
                { new Guid("fe49360e-9d5c-4c0f-9b16-bdd1bd6242f5"), "London" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Cities");
    }
}
