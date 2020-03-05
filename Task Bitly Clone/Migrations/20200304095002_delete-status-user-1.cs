using Microsoft.EntityFrameworkCore.Migrations;

namespace Task_Bitly_Clone.Migrations
{
    public partial class deletestatususer1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
