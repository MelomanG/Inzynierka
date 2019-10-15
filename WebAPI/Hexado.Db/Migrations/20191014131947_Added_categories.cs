using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hexado.Db.Migrations
{
    public partial class Added_categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "BoardGames",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoardGamesCategories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardGamesCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardGames_CategoryId",
                table: "BoardGames",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardGamesCategories_Name",
                table: "BoardGamesCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGames_BoardGamesCategories_CategoryId",
                table: "BoardGames",
                column: "CategoryId",
                principalTable: "BoardGamesCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGames_BoardGamesCategories_CategoryId",
                table: "BoardGames");

            migrationBuilder.DropTable(
                name: "BoardGamesCategories");

            migrationBuilder.DropIndex(
                name: "IX_BoardGames_CategoryId",
                table: "BoardGames");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BoardGames");
        }
    }
}
