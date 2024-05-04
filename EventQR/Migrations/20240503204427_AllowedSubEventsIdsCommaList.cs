using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventQR.Migrations
{
    /// <inheritdoc />
    public partial class AllowedSubEventsIdsCommaList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedSubEventsIdsCommaList",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedSubEventsIdsCommaList",
                table: "Guests");
        }
    }
}
