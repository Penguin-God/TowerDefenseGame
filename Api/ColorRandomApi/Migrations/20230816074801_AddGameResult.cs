using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ColorRandomApi.Migrations
{
    public partial class AddGameResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName1 = table.Column<string>(nullable: true),
                    SkillName2 = table.Column<string>(nullable: true),
                    GameTime = table.Column<DateTime>(nullable: false),
                    winSkill = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResults", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameResults");
        }
    }
}
