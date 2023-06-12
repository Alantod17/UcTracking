using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class ChangeTimeToUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileLastWriteDateTime",
                table: "FileEntryEncryptionLog",
                newName: "FileLastWriteUtcDateTime");

            migrationBuilder.RenameColumn(
                name: "LastWriteDateTime",
                table: "FileEntry",
                newName: "LastWriteUtcDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileLastWriteUtcDateTime",
                table: "FileEntryEncryptionLog",
                newName: "FileLastWriteDateTime");

            migrationBuilder.RenameColumn(
                name: "LastWriteUtcDateTime",
                table: "FileEntry",
                newName: "LastWriteDateTime");
        }
    }
}
