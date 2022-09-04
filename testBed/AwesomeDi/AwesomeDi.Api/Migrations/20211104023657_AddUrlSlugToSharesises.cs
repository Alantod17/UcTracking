using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class AddUrlSlugToSharesises : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlSlug",
                table: "SharesiesInstrument",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlSlug",
                table: "SharesiesInstrument");
        }
    }
}
