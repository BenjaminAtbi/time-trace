using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace time_trace.Migrations
{
    public partial class addScheduleOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_AspNetUsers_ApplicationUserId",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Schedules",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_ApplicationUserId",
                table: "Schedules",
                newName: "IX_Schedules_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_AspNetUsers_OwnerId",
                table: "Schedules",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_AspNetUsers_OwnerId",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Schedules",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_OwnerId",
                table: "Schedules",
                newName: "IX_Schedules_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_AspNetUsers_ApplicationUserId",
                table: "Schedules",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
