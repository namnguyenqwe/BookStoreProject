using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreProject.Migrations
{
    public partial class ChangeInformationInBookTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Infomation",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Infomation",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
