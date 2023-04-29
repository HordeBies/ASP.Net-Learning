using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CitiesManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    { new Guid("6bd9f2a9-3f22-47cb-999d-fdd1ab7790aa"), "Tokyo" },
                    { new Guid("8207dd6c-2504-46ba-9dc4-c2361f6f4e1f"), "London" },
                    { new Guid("8d9188e9-b249-4d2b-b564-1fe0e1997bc1"), "Paris" },
                    { new Guid("9364a092-aad7-48dc-9a71-4a4cf6e02211"), "Hong Kong" },
                    { new Guid("d14bc922-c581-4181-b23c-5a86a6177c43"), "New York" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
