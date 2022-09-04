using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeDi.Api.Migrations
{
    public partial class AddIndexToTrackingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TrackingUiData_TrackingId_DateTime",
                table: "TrackingUiData",
                columns: new[] { "TrackingId", "DateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingRequestData_TrackingId_DateTime",
                table: "TrackingRequestData",
                columns: new[] { "TrackingId", "DateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingEventData_TrackingId_DateTime",
                table: "TrackingEventData",
                columns: new[] { "TrackingId", "DateTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackingUiData_TrackingId_DateTime",
                table: "TrackingUiData");

            migrationBuilder.DropIndex(
                name: "IX_TrackingRequestData_TrackingId_DateTime",
                table: "TrackingRequestData");

            migrationBuilder.DropIndex(
                name: "IX_TrackingEventData_TrackingId_DateTime",
                table: "TrackingEventData");
        }
    }
}
