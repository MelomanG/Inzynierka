using Microsoft.EntityFrameworkCore.Migrations;

namespace Hexado.Db.Migrations
{
    public partial class EventAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressId",
                table: "Event",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Event_AddressId",
                table: "Event",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Addresses_AddressId",
                table: "Event",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Addresses_AddressId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_AddressId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Event");
        }
    }
}
