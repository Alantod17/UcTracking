using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class AddResearchDetialFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentFilePath",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchBugRootCause",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchChallenges",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchContext",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchContributionType",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchDataSource",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchFutureWorks",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGoal",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchMainFindings",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchPlatform",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchQuestions",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchedApproach",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchedProblemType",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchedTechnology",
                table: "ResearchArticle",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentFilePath",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchBugRootCause",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchChallenges",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchContext",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchContributionType",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchDataSource",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchFutureWorks",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchGoal",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchMainFindings",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchPlatform",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchQuestions",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchedApproach",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchedProblemType",
                table: "ResearchArticle");

            migrationBuilder.DropColumn(
                name: "ResearchedTechnology",
                table: "ResearchArticle");
        }
    }
}
