using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityTracer.Migrations
{
    /// <inheritdoc />
    public partial class Owner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_OwnerId",
                table: "Activities",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_OwnerId",
                table: "Activities",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_OwnerId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_OwnerId",
                table: "Activities");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
