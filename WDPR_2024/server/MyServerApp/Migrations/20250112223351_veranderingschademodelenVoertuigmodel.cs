using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyServerApp.Migrations
{
    /// <inheritdoc />
    public partial class veranderingschademodelenVoertuigmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Opmerkingen",
                table: "Voertuigen",
                newName: "HuidigeHuurderNaam");

            migrationBuilder.AddColumn<string>(
                name: "HuidigeHuurderEmail",
                table: "Voertuigen",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HuidigeHuurderEmail",
                table: "Voertuigen");

            migrationBuilder.RenameColumn(
                name: "HuidigeHuurderNaam",
                table: "Voertuigen",
                newName: "Opmerkingen");
        }
    }
}
