using Microsoft.EntityFrameworkCore.Migrations;

namespace ColorRandomApi.Migrations
{
    public partial class add_gold_gem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gem",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "Players",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gem",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "Players");
        }
    }
}
