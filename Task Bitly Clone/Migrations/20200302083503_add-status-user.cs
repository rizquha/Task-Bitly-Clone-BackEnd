using Microsoft.EntityFrameworkCore.Migrations;

namespace Task_Bitly_Clone.Migrations
{
    public partial class addstatususer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "users");
        }
    }
}
