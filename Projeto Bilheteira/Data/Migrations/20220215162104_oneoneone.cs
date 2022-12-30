using Microsoft.EntityFrameworkCore.Migrations;

namespace Utad_Proj_.Data.Migrations
{
    public partial class oneoneone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "trailer",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "trailer",
                table: "Movies");
        }
    }
}
