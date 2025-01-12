using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOpmerkingenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opmerkingen",
                table: "VerhuurAanvragen");

            migrationBuilder.CreateTable(
                name: "Opmerkingen",
                columns: table => new
                {
                    OpmerkingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerhuurAanvraagID = table.Column<int>(type: "int", nullable: false),
                    GebruikerNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumToegevoegd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opmerkingen", x => x.OpmerkingID);
                    table.ForeignKey(
                        name: "FK_Opmerkingen_VerhuurAanvragen_VerhuurAanvraagID",
                        column: x => x.VerhuurAanvraagID,
                        principalTable: "VerhuurAanvragen",
                        principalColumn: "VerhuurAanvraagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opmerkingen_VerhuurAanvraagID",
                table: "Opmerkingen",
                column: "VerhuurAanvraagID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opmerkingen");

            migrationBuilder.AddColumn<string>(
                name: "Opmerkingen",
                table: "VerhuurAanvragen",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
