using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebApp.Migrations
{
    public partial class MigrateDB10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "VideoData",
                table: "Comments",
                type: "BLOB",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogArticleVideo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    VideoData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    VideoName = table.Column<string>(type: "TEXT", nullable: true),
                    BlogArticleId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogArticleVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogArticleVideo_BlogArticles_BlogArticleId",
                        column: x => x.BlogArticleId,
                        principalTable: "BlogArticles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogArticleVideo_BlogArticleId",
                table: "BlogArticleVideo",
                column: "BlogArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogArticleVideo");

            migrationBuilder.DropColumn(
                name: "VideoData",
                table: "Comments");
        }
    }
}
