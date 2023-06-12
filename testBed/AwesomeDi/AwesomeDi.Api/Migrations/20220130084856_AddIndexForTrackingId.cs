using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeDi.Api.Migrations
{
    public partial class AddIndexForTrackingId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrackingId",
                table: "TrackingUiData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrackingId",
                table: "TrackingRequestData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUiData_TrackingId",
                table: "TrackingUiData",
                column: "TrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingRequestData_TrackingId",
                table: "TrackingRequestData",
                column: "TrackingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackingUiData_TrackingId",
                table: "TrackingUiData");

            migrationBuilder.DropIndex(
                name: "IX_TrackingRequestData_TrackingId",
                table: "TrackingRequestData");

            migrationBuilder.AlterColumn<int>(
                name: "TrackingId",
                table: "TrackingUiData",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrackingId",
                table: "TrackingRequestData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
