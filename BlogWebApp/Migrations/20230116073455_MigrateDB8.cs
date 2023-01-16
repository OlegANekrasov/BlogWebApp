using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebApp.Migrations
{
    public partial class MigrateDB8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfVisit",
                table: "BlogArticles",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfVisit",
                table: "BlogArticles");
        }
    }
}
