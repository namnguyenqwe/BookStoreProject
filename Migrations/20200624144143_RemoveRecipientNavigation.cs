using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreProject.Migrations
{
    public partial class RemoveRecipientNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipient_AspNetUsers_ApplicationUserId",
                table: "Recipient");

            migrationBuilder.DropIndex(
                name: "IX_Recipient_ApplicationUserId",
                table: "Recipient");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Recipient");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Recipient",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_ApplicationUserId",
                table: "Recipient",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipient_AspNetUsers_ApplicationUserId",
                table: "Recipient",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
