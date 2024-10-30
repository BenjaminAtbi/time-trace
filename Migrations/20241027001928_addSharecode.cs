using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace time_trace.Migrations
{
    public partial class addSharecode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Sharecode",
                table: "Schedules",
                type: "uuid",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sharecode",
                table: "Schedules");
        }
    }
}
