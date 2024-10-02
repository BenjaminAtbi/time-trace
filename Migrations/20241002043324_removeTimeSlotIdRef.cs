using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace time_trace.Migrations
{
    public partial class removeTimeSlotIdRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlots_UserSchedules_UserScheduleScheduleId_UserSchedule~",
                table: "TimeSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSchedules",
                table: "UserSchedules");

            migrationBuilder.DropIndex(
                name: "IX_UserSchedules_UserId",
                table: "UserSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSlots",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlots_UserScheduleScheduleId_UserScheduleUserId",
                table: "TimeSlots");

            migrationBuilder.DropColumn(
                name: "UserScheduleId",
                table: "TimeSlots");

            migrationBuilder.RenameColumn(
                name: "UserScheduleUserId",
                table: "TimeSlots",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UserScheduleScheduleId",
                table: "TimeSlots",
                newName: "ScheduleId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Schedules",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSchedules",
                table: "UserSchedules",
                columns: new[] { "UserId", "ScheduleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSlots",
                table: "TimeSlots",
                columns: new[] { "DateTime", "UserId", "ScheduleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSchedules_ScheduleId",
                table: "UserSchedules",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_UserId_ScheduleId",
                table: "TimeSlots",
                columns: new[] { "UserId", "ScheduleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ApplicationUserId",
                table: "Schedules",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_AspNetUsers_ApplicationUserId",
                table: "Schedules",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlots_UserSchedules_UserId_ScheduleId",
                table: "TimeSlots",
                columns: new[] { "UserId", "ScheduleId" },
                principalTable: "UserSchedules",
                principalColumns: new[] { "UserId", "ScheduleId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_AspNetUsers_ApplicationUserId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlots_UserSchedules_UserId_ScheduleId",
                table: "TimeSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSchedules",
                table: "UserSchedules");

            migrationBuilder.DropIndex(
                name: "IX_UserSchedules_ScheduleId",
                table: "UserSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSlots",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlots_UserId_ScheduleId",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_ApplicationUserId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "TimeSlots",
                newName: "UserScheduleScheduleId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TimeSlots",
                newName: "UserScheduleUserId");

            migrationBuilder.AddColumn<int>(
                name: "UserScheduleId",
                table: "TimeSlots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSchedules",
                table: "UserSchedules",
                columns: new[] { "ScheduleId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSlots",
                table: "TimeSlots",
                columns: new[] { "UserScheduleId", "DateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSchedules_UserId",
                table: "UserSchedules",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_UserScheduleScheduleId_UserScheduleUserId",
                table: "TimeSlots",
                columns: new[] { "UserScheduleScheduleId", "UserScheduleUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlots_UserSchedules_UserScheduleScheduleId_UserSchedule~",
                table: "TimeSlots",
                columns: new[] { "UserScheduleScheduleId", "UserScheduleUserId" },
                principalTable: "UserSchedules",
                principalColumns: new[] { "ScheduleId", "UserId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
