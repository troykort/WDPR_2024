using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class schademeldingenmodelenverhuuraanvraagverandering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_Klanten_KlantID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_Voertuigen_VoertuigID",
                table: "Schademeldingen");

            migrationBuilder.AddColumn<int>(
                name: "SchademeldingID",
                table: "VerhuurAanvragen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchademeldingID1",
                table: "VerhuurAanvragen",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KlantID",
                table: "Schademeldingen",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_VerhuurAanvragen_SchademeldingID1",
                table: "VerhuurAanvragen",
                column: "SchademeldingID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_Klanten_KlantID",
                table: "Schademeldingen",
                column: "KlantID",
                principalTable: "Klanten",
                principalColumn: "KlantID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Schademeldingen",
                column: "VerhuurAanvraagID",
                principalTable: "VerhuurAanvragen",
                principalColumn: "VerhuurAanvraagID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_Voertuigen_VoertuigID",
                table: "Schademeldingen",
                column: "VoertuigID",
                principalTable: "Voertuigen",
                principalColumn: "VoertuigID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerhuurAanvragen_Schademeldingen_SchademeldingID1",
                table: "VerhuurAanvragen",
                column: "SchademeldingID1",
                principalTable: "Schademeldingen",
                principalColumn: "SchademeldingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_Klanten_KlantID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Schademeldingen_Voertuigen_VoertuigID",
                table: "Schademeldingen");

            migrationBuilder.DropForeignKey(
                name: "FK_VerhuurAanvragen_Schademeldingen_SchademeldingID1",
                table: "VerhuurAanvragen");

            migrationBuilder.DropIndex(
                name: "IX_VerhuurAanvragen_SchademeldingID1",
                table: "VerhuurAanvragen");

            migrationBuilder.DropColumn(
                name: "SchademeldingID",
                table: "VerhuurAanvragen");

            migrationBuilder.DropColumn(
                name: "SchademeldingID1",
                table: "VerhuurAanvragen");

            migrationBuilder.AlterColumn<int>(
                name: "KlantID",
                table: "Schademeldingen",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_Klanten_KlantID",
                table: "Schademeldingen",
                column: "KlantID",
                principalTable: "Klanten",
                principalColumn: "KlantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_VerhuurAanvragen_VerhuurAanvraagID",
                table: "Schademeldingen",
                column: "VerhuurAanvraagID",
                principalTable: "VerhuurAanvragen",
                principalColumn: "VerhuurAanvraagID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schademeldingen_Voertuigen_VoertuigID",
                table: "Schademeldingen",
                column: "VoertuigID",
                principalTable: "Voertuigen",
                principalColumn: "VoertuigID");
        }
    }
}
