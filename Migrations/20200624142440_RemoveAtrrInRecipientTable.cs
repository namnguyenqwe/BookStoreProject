using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreProject.Migrations
{
    public partial class RemoveAtrrInRecipientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipient_AspNetUsers_ApplicationUserID",
                table: "Recipient");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
                table: "Recipient",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipient_ApplicationUserID",
                table: "Recipient",
                newName: "IX_Recipient_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipient_AspNetUsers_ApplicationUserId",
                table: "Recipient",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipient_AspNetUsers_ApplicationUserId",
                table: "Recipient");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Recipient",
                newName: "ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Recipient_ApplicationUserId",
                table: "Recipient",
                newName: "IX_Recipient_ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipient_AspNetUsers_ApplicationUserID",
                table: "Recipient",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
