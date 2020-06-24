using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreProject.Migrations
{
    public partial class UPdaterecipent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "Recipient",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_ApplicationUserID",
                table: "Recipient",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipient_AspNetUsers_ApplicationUserID",
                table: "Recipient",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipient_AspNetUsers_ApplicationUserID",
                table: "Recipient");

            migrationBuilder.DropIndex(
                name: "IX_Recipient_ApplicationUserID",
                table: "Recipient");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "Recipient");
        }
    }
}
