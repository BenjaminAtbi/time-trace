using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace time_trace.Migrations
{
    public partial class setOwnerRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_AspNetUsers_OwnerId",
                table: "Schedules");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Schedules",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_AspNetUsers_OwnerId",
                table: "Schedules",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_AspNetUsers_OwnerId",
                table: "Schedules");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Schedules",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_AspNetUsers_OwnerId",
                table: "Schedules",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
