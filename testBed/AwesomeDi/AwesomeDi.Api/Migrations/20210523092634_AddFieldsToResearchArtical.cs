using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class AddFieldsToResearchArtical : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ResearchArticle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDuplicate",
                table: "ResearchArticle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIrrelevant",
                table: "ResearchArticle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNeedReview",
                table: "ResearchArticle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "IsDuplicate",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "IsIrrelevant",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "IsNeedReview",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ResearchArticle");
        }
    }
}
