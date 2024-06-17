using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityTracer.Migrations
{
    /// <inheritdoc />
    public partial class UserFollow_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserFollows",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserFollows");
        }
    }
}
