using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class addrolltoklant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Klanten_UserID",
                table: "Klanten");

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Klanten",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Klanten_UserID",
                table: "Klanten",
                column: "UserID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Klanten_UserID",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Klanten");

            migrationBuilder.CreateIndex(
                name: "IX_Klanten_UserID",
                table: "Klanten",
                column: "UserID");
        }
    }
}
