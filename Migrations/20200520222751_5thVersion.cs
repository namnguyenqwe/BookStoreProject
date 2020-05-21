using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreProject.Migrations
{
    public partial class _5thVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DistrictID",
                table: "Recipient",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CityID",
                table: "Recipient",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityID = table.Column<string>(nullable: false),
                    city = table.Column<string>(nullable: true),
                    type = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityID);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    DistrictID = table.Column<string>(nullable: false),
                    district = table.Column<string>(nullable: true),
                    type = table.Column<string>(maxLength: 30, nullable: true),
                    CityID = table.Column<string>(nullable: true),
                    Fee = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.DistrictID);
                    table.ForeignKey(
                        name: "FK_District_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_CityID",
                table: "Recipient",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_DistrictID",
                table: "Recipient",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_District_CityID",
                table: "District",
                column: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipient_City_CityID",
                table: "Recipient",
                column: "CityID",
                principalTable: "City",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipient_District_DistrictID",
                table: "Recipient",
                column: "DistrictID",
                principalTable: "District",
                principalColumn: "DistrictID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipient_City_CityID",
                table: "Recipient");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipient_District_DistrictID",
                table: "Recipient");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropIndex(
                name: "IX_Recipient_CityID",
                table: "Recipient");

            migrationBuilder.DropIndex(
                name: "IX_Recipient_DistrictID",
                table: "Recipient");

            migrationBuilder.AlterColumn<string>(
                name: "DistrictID",
                table: "Recipient",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CityID",
                table: "Recipient",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
