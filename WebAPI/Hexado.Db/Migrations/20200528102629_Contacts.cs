using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hexado.Db.Migrations
{
    public partial class Contacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    ContactHexadoUserId = table.Column<string>(nullable: false),
                    HexadoUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.UniqueConstraint("AK_Contacts_HexadoUserId_ContactHexadoUserId", x => new { x.HexadoUserId, x.ContactHexadoUserId });
                    table.ForeignKey(
                        name: "FK_Contacts_AspNetUsers_ContactHexadoUserId",
                        column: x => x.ContactHexadoUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contacts_AspNetUsers_HexadoUserId",
                        column: x => x.HexadoUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ContactHexadoUserId",
                table: "Contacts",
                column: "ContactHexadoUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
