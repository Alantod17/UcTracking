using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastWriteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "FileEntryEncryptionLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileEntryId = table.Column<int>(type: "int", nullable: false),
                    FileLastWriteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EncryptedFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntryEncryptionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileEntryEncryptionLog_FileEntry_FileEntryId",
                        column: x => x.FileEntryId,
                        principalTable: "FileEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileEntryEncryptionLog_FileEntryId",
                table: "FileEntryEncryptionLog",
                column: "FileEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileEntryEncryptionLog");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "FileEntry");
        }
    }
}
