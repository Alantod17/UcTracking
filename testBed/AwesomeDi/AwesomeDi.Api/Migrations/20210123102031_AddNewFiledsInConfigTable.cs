using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class AddNewFiledsInConfigTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailPath",
                table: "Configuration",
                newName: "ThumbnailFolderPath");

            migrationBuilder.AddColumn<string>(
                name: "DefaultThumbnailFilePath",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultThumbnailFilePath",
                table: "Configuration");

            migrationBuilder.RenameColumn(
                name: "ThumbnailFolderPath",
                table: "Configuration",
                newName: "ThumbnailPath");
        }
    }
}
