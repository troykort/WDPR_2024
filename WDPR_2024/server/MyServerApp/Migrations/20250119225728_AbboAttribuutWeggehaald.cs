using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AbboAttribuutWeggehaald : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToeslagVoorPremiumVoertuigen",
                table: "Abonnementen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ToeslagVoorPremiumVoertuigen",
                table: "Abonnementen",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
