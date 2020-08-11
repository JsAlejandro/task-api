using Microsoft.EntityFrameworkCore.Migrations;

namespace taskmanager_api.Migrations
{
    public partial class addAssigneeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssigneeId",
                table: "Assignment",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Assignment");
        }
    }
}
