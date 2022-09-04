using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class addFileSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "FileEntry",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "FileEntry");
        }
    }
}
