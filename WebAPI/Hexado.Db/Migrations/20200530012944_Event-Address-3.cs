using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hexado.Db.Migrations
{
    public partial class EventAddress3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Addresses_AddressId",
                table: "Event");

            migrationBuilder.CreateTable(
                name: "EventAddress",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    BuildingNumber = table.Column<string>(nullable: true),
                    LocalNumber = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAddress", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventAddress_AddressId",
                table: "Event",
                column: "AddressId",
                principalTable: "EventAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventAddress_AddressId",
                table: "Event");

            migrationBuilder.DropTable(
                name: "EventAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Addresses_AddressId",
                table: "Event",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
