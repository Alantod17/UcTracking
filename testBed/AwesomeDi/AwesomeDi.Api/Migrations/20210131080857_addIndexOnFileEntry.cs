using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class addIndexOnFileEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FileEntry_LastWriteUtcDateTime",
                table: "FileEntry",
                column: "LastWriteUtcDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileEntry_LastWriteUtcDateTime",
                table: "FileEntry");
        }
    }
}
