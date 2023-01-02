using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebApp.Migrations
{
    public partial class MigrateDB5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogArticle_AspNetUsers_UserId",
                table: "BlogArticle");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogArticleTag_BlogArticle_BlogArticlesId",
                table: "BlogArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogArticleTag_Tag_TagsId",
                table: "BlogArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_BlogArticle_BlogArticleId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogArticle",
                table: "BlogArticle");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "BlogArticle",
                newName: "BlogArticles");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_BlogArticleId",
                table: "Comments",
                newName: "IX_Comments_BlogArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogArticle_UserId",
                table: "BlogArticles",
                newName: "IX_BlogArticles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogArticles",
                table: "BlogArticles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogArticles_AspNetUsers_UserId",
                table: "BlogArticles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogArticleTag_BlogArticles_BlogArticlesId",
                table: "BlogArticleTag",
                column: "BlogArticlesId",
                principalTable: "BlogArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogArticleTag_Tags_TagsId",
                table: "BlogArticleTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_BlogArticles_BlogArticleId",
                table: "Comments",
                column: "BlogArticleId",
                principalTable: "BlogArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogArticles_AspNetUsers_UserId",
                table: "BlogArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogArticleTag_BlogArticles_BlogArticlesId",
                table: "BlogArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogArticleTag_Tags_TagsId",
                table: "BlogArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_BlogArticles_BlogArticleId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogArticles",
                table: "BlogArticles");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "BlogArticles",
                newName: "BlogArticle");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comment",
                newName: "IX_Comment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BlogArticleId",
                table: "Comment",
                newName: "IX_Comment_BlogArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogArticles_UserId",
                table: "BlogArticle",
                newName: "IX_BlogArticle_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogArticle",
                table: "BlogArticle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogArticle_AspNetUsers_UserId",
                table: "BlogArticle",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogArticleTag_BlogArticle_BlogArticlesId",
                table: "BlogArticleTag",
                column: "BlogArticlesId",
                principalTable: "BlogArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogArticleTag_Tag_TagsId",
                table: "BlogArticleTag",
                column: "TagsId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_BlogArticle_BlogArticleId",
                table: "Comment",
                column: "BlogArticleId",
                principalTable: "BlogArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
