using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class notificatieverandering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaties_Klanten_KlantID",
                table: "Notificaties");

            migrationBuilder.DropForeignKey(
                name: "FK_Opmerkingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Opmerkingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_Klanten_KlantID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_Voertuigen_VoertuigID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_VerhuurAanvragen_Klanten_KlantID",
                table: "VerhuurAanvragen");

            migrationBuilder.DropForeignKey(
                name: "FK_VerhuurAanvragen_Voertuigen_VoertuigID",
                table: "VerhuurAanvragen");

            migrationBuilder.AlterColumn<int>(
                name: "KlantID",
                table: "Notificaties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MedewerkerID",
                table: "Notificaties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificaties_MedewerkerID",
                table: "Notificaties",
                column: "MedewerkerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaties_Klanten_KlantID",
                table: "Notificaties",
                column: "KlantID",
                principalTable: "Klanten",
                principalColumn: "KlantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaties_Medewerkers_MedewerkerID",
                table: "Notificaties",
                column: "MedewerkerID",
                principalTable: "Medewerkers",
                principalColumn: "MedewerkerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Opmerkingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Opmerkingen",
                column: "VerhuurAanvraagID",
                principalTable: "VerhuurAanvragen",
                principalColumn: "VerhuurAanvraagID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_Klanten_KlantID",
                table: "Schademeldingen",
                column: "KlantID",
                principalTable: "Klanten",
                principalColumn: "KlantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_Voertuigen_VoertuigID",
                table: "Schademeldingen",
                column: "VoertuigID",
                principalTable: "Voertuigen",
                principalColumn: "VoertuigID");

            migrationBuilder.AddForeignKey(
                name: "FK_VerhuurAanvragen_Klanten_KlantID",
                table: "VerhuurAanvragen",
                column: "KlantID",
                principalTable: "Klanten",
                principalColumn: "KlantID");

            migrationBuilder.AddForeignKey(
                name: "FK_VerhuurAanvragen_Voertuigen_VoertuigID",
                table: "VerhuurAanvragen",
                column: "VoertuigID",
                principalTable: "Voertuigen",
                principalColumn: "VoertuigID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaties_Klanten_KlantID",
                table: "Notificaties");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaties_Medewerkers_MedewerkerID",
                table: "Notificaties");

            migrationBuilder.DropForeignKey(
                name: "FK_Opmerkingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Opmerkingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_Klanten_KlantID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_Voertuigen_VoertuigID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_VerhuurAanvragen_Klanten_KlantID",
                table: "VerhuurAanvragen");

            migrationBuilder.DropForeignKey(
                name: "FK_VerhuurAanvragen_Voertuigen_VoertuigID",
                table: "VerhuurAanvragen");

            migrationBuilder.DropIndex(
                name: "IX_Notificaties_MedewerkerID",
                table: "Notificaties");

            migrationBuilder.DropColumn(
                name: "MedewerkerID",
                table: "Notificaties");

            migrationBuilder.AlterColumn<int>(
                name: "KlantID",
                table: "Notificaties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaties_Klanten_KlantID",
                table: "Notificaties",
                column: "KlantID",
                principalTable: "Klanten",
                principalColumn: "KlantID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Opmerkingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Opmerkingen",
                column: "VerhuurAanvraagID",
                principalTable: "VerhuurAanvragen",
                principalColumn: "VerhuurAanvraagID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_Klanten_KlantID",
                table: "Schademeldingen",
                column: "KlantID",
                principalTable: "Klanten",
                principalColumn: "KlantID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_Voertuigen_VoertuigID",
                table: "Schademeldingen",
                column: "VoertuigID",
                principalTable: "Voertuigen",
                principalColumn: "VoertuigID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerhuurAanvragen_Klanten_KlantID",
                table: "VerhuurAanvragen",
                column: "KlantID",
                principalTable: "Klanten",
                principalColumn: "KlantID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerhuurAanvragen_Voertuigen_VoertuigID",
                table: "VerhuurAanvragen",
                column: "VoertuigID",
                principalTable: "Voertuigen",
                principalColumn: "VoertuigID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
