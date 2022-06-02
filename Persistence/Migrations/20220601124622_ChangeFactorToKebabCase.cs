using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ChangeFactorToKebabCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Multiplier_Factor",
                table: "Event",
                newName: "MultiplierFactor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MultiplierFactor",
                table: "Event",
                newName: "Multiplier_Factor");
        }
    }
}
