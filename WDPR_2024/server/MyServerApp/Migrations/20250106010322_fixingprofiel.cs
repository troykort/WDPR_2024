using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class fixingprofiel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Medewerkers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Klanten",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Medewerkers_UserID",
                table: "Medewerkers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Klanten_UserID",
                table: "Klanten",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Klanten_AspNetUsers_UserID",
                table: "Klanten",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medewerkers_AspNetUsers_UserID",
                table: "Medewerkers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Klanten_AspNetUsers_UserID",
                table: "Klanten");

            migrationBuilder.DropForeignKey(
                name: "FK_Medewerkers_AspNetUsers_UserID",
                table: "Medewerkers");

            migrationBuilder.DropIndex(
                name: "IX_Medewerkers_UserID",
                table: "Medewerkers");

            migrationBuilder.DropIndex(
                name: "IX_Klanten_UserID",
                table: "Klanten");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Medewerkers");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Klanten");
        }
    }
}
