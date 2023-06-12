using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeDi.Api.Migrations
{
    public partial class AddEventType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "TrackingUiData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "TrackingUiData");
        }
    }
}
