using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hexado.Db.Migrations
{
    public partial class Likes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikedBoardGames",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    BoardGameId = table.Column<string>(nullable: false),
                    HexadoUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedBoardGames", x => x.Id);
                    table.UniqueConstraint("AK_LikedBoardGames_BoardGameId_HexadoUserId", x => new { x.BoardGameId, x.HexadoUserId });
                    table.ForeignKey(
                        name: "FK_LikedBoardGames_BoardGames_BoardGameId",
                        column: x => x.BoardGameId,
                        principalTable: "BoardGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedBoardGames_AspNetUsers_HexadoUserId",
                        column: x => x.HexadoUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LikedPubs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    PubId = table.Column<string>(nullable: false),
                    HexadoUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedPubs", x => x.Id);
                    table.UniqueConstraint("AK_LikedPubs_PubId_HexadoUserId", x => new { x.PubId, x.HexadoUserId });
                    table.ForeignKey(
                        name: "FK_LikedPubs_AspNetUsers_HexadoUserId",
                        column: x => x.HexadoUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LikedPubs_Pubs_PubId",
                        column: x => x.PubId,
                        principalTable: "Pubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikedBoardGames_HexadoUserId",
                table: "LikedBoardGames",
                column: "HexadoUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedPubs_HexadoUserId",
                table: "LikedPubs",
                column: "HexadoUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikedBoardGames");

            migrationBuilder.DropTable(
                name: "LikedPubs");
        }
    }
}
