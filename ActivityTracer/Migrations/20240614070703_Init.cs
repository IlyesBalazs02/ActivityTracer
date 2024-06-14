using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityTracer.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SelectedSport = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: true),
                    Elevation = table.Column<int>(type: "int", nullable: true),
                    Pace = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Distance = table.Column<int>(type: "int", nullable: true),
                    AvgHeartRate = table.Column<int>(type: "int", nullable: true),
                    MaxHeartRate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
