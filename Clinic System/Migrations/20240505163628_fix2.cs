using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_System.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPhoneNumbers_AspNetUsers_UserId",
                table: "UserPhoneNumbers");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhoneNumbers_AspNetUsers_UserId",
                table: "UserPhoneNumbers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPhoneNumbers_AspNetUsers_UserId",
                table: "UserPhoneNumbers");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhoneNumbers_AspNetUsers_UserId",
                table: "UserPhoneNumbers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
