using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreProject.Migrations
{
    public partial class AddBookWeightAtribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Book");
        }
    }
}
