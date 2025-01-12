using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class veranderingschademodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VerhuurAanvraagID",
                table: "Schademeldingen",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schademeldingen_VerhuurAanvraagID",
                table: "Schademeldingen",
                column: "VerhuurAanvraagID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Schademeldingen",
                column: "VerhuurAanvraagID",
                principalTable: "VerhuurAanvragen",
                principalColumn: "VerhuurAanvraagID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Schademeldingen");

            migrationBuilder.DropIndex(
                name: "IX_Schademeldingen_VerhuurAanvraagID",
                table: "Schademeldingen");

            migrationBuilder.DropColumn(
                name: "VerhuurAanvraagID",
                table: "Schademeldingen");
        }
    }
}
