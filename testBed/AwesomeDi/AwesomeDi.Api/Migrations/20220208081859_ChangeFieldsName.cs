using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeDi.Api.Migrations
{
    public partial class ChangeFieldsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScreenHref",
                table: "TrackingRequestData",
                newName: "Href");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "TrackingEventData",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Event",
                table: "TrackingEventData",
                newName: "Href");

            migrationBuilder.AddColumn<string>(
                name: "Href",
                table: "TrackingUiData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventDetail",
                table: "TrackingEventData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Href",
                table: "TrackingUiData");

            migrationBuilder.DropColumn(
                name: "EventDetail",
                table: "TrackingEventData");

            migrationBuilder.RenameColumn(
                name: "Href",
                table: "TrackingRequestData",
                newName: "ScreenHref");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "TrackingEventData",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "Href",
                table: "TrackingEventData",
                newName: "Event");
        }
    }
}
