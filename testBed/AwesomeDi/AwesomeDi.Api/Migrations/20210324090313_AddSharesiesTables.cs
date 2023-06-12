using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeDi.Api.Migrations
{
    public partial class AddSharesiesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SharesiesConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharesiesConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharesiesInstrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstrumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskRating = table.Column<int>(type: "int", nullable: false),
                    ExchangeCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SharesiesId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharesiesInstrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharesiesInstrumentCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharesiesInstrumentCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharesiesInstrumentComparisonPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Percent = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Max = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Min = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    SharesiesInstrumentId = table.Column<int>(type: "int", nullable: false),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharesiesInstrumentComparisonPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharesiesInstrumentComparisonPrice_SharesiesInstrument_SharesiesInstrumentId",
                        column: x => x.SharesiesInstrumentId,
                        principalTable: "SharesiesInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharesiesInstrumentPriceHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    SharesiesInstrumentId = table.Column<int>(type: "int", nullable: false),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharesiesInstrumentPriceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharesiesInstrumentPriceHistory_SharesiesInstrument_SharesiesInstrumentId",
                        column: x => x.SharesiesInstrumentId,
                        principalTable: "SharesiesInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharesiesInstrumentXCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SharesiesInstrumentId = table.Column<int>(type: "int", nullable: false),
                    SharesiesInstrumentCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharesiesInstrumentXCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharesiesInstrumentXCategory_SharesiesInstrument_SharesiesInstrumentId",
                        column: x => x.SharesiesInstrumentId,
                        principalTable: "SharesiesInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharesiesInstrumentXCategory_SharesiesInstrumentCategory_SharesiesInstrumentCategoryId",
                        column: x => x.SharesiesInstrumentCategoryId,
                        principalTable: "SharesiesInstrumentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharesiesInstrumentComparisonPrice_SharesiesInstrumentId",
                table: "SharesiesInstrumentComparisonPrice",
                column: "SharesiesInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SharesiesInstrumentPriceHistory_SharesiesInstrumentId",
                table: "SharesiesInstrumentPriceHistory",
                column: "SharesiesInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SharesiesInstrumentXCategory_SharesiesInstrumentCategoryId",
                table: "SharesiesInstrumentXCategory",
                column: "SharesiesInstrumentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SharesiesInstrumentXCategory_SharesiesInstrumentId",
                table: "SharesiesInstrumentXCategory",
                column: "SharesiesInstrumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharesiesConfiguration");

            migrationBuilder.DropTable(
                name: "SharesiesInstrumentComparisonPrice");

            migrationBuilder.DropTable(
                name: "SharesiesInstrumentPriceHistory");

            migrationBuilder.DropTable(
                name: "SharesiesInstrumentXCategory");

            migrationBuilder.DropTable(
                name: "SharesiesInstrument");

            migrationBuilder.DropTable(
                name: "SharesiesInstrumentCategory");
        }
    }
}
