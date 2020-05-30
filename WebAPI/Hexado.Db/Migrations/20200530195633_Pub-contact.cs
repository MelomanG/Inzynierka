using Microsoft.EntityFrameworkCore.Migrations;

namespace Hexado.Db.Migrations
{
    public partial class Pubcontact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Pubs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Pubs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Pubs");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Pubs");
        }
    }
}
