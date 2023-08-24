using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pages.App.Migrations
{
    public partial class updatedsponsor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sponsors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sponsors");
        }
    }
}
