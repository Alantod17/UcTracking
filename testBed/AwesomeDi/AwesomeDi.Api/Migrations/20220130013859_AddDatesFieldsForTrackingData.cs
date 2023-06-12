using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeDi.Api.Migrations
{
    public partial class AddDatesFieldsForTrackingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtcDateTime",
                table: "TrackingUiData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedUtcDateTime",
                table: "TrackingUiData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtcDateTime",
                table: "TrackingRequestData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedUtcDateTime",
                table: "TrackingRequestData",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUtcDateTime",
                table: "TrackingUiData");

            migrationBuilder.DropColumn(
                name: "ModifiedUtcDateTime",
                table: "TrackingUiData");

            migrationBuilder.DropColumn(
                name: "CreatedUtcDateTime",
                table: "TrackingRequestData");

            migrationBuilder.DropColumn(
                name: "ModifiedUtcDateTime",
                table: "TrackingRequestData");
        }
    }
}
