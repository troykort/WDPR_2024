using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAbbonnement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AantalHuurdagenPerJaar",
                table: "Abonnementen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "KortingOpVoertuighuur",
                table: "Abonnementen",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "KostenPerJaar",
                table: "Abonnementen",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaandelijkseAbonnementskosten",
                table: "Abonnementen",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OvergebruikKostenPerDag",
                table: "Abonnementen",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ToeslagVoorPremiumVoertuigen",
                table: "Abonnementen",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AantalHuurdagenPerJaar",
                table: "Abonnementen");

            migrationBuilder.DropColumn(
                name: "KortingOpVoertuighuur",
                table: "Abonnementen");

            migrationBuilder.DropColumn(
                name: "KostenPerJaar",
                table: "Abonnementen");

            migrationBuilder.DropColumn(
                name: "MaandelijkseAbonnementskosten",
                table: "Abonnementen");

            migrationBuilder.DropColumn(
                name: "OvergebruikKostenPerDag",
                table: "Abonnementen");

            migrationBuilder.DropColumn(
                name: "ToeslagVoorPremiumVoertuigen",
                table: "Abonnementen");
        }
    }
}
