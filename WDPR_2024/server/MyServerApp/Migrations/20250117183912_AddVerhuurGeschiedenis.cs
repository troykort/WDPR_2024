using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddVerhuurGeschiedenis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerhuurGeschiedenis",
                columns: table => new
                {
                    VerhuurGeschiedenisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlantID = table.Column<int>(type: "int", nullable: false),
                    VoertuigID = table.Column<int>(type: "int", nullable: false),
                    StartDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotaleKosten = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerhuurGeschiedenis", x => x.VerhuurGeschiedenisID);
                    table.ForeignKey(
                        name: "FK_VerhuurGeschiedenis_Klanten_KlantID",
                        column: x => x.KlantID,
                        principalTable: "Klanten",
                        principalColumn: "KlantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VerhuurGeschiedenis_Voertuigen_VoertuigID",
                        column: x => x.VoertuigID,
                        principalTable: "Voertuigen",
                        principalColumn: "VoertuigID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerhuurGeschiedenis_KlantID",
                table: "VerhuurGeschiedenis",
                column: "KlantID");

            migrationBuilder.CreateIndex(
                name: "IX_VerhuurGeschiedenis_VoertuigID",
                table: "VerhuurGeschiedenis",
                column: "VoertuigID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerhuurGeschiedenis");
        }
    }
}
