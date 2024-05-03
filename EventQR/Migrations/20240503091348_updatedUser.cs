using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventQR.Migrations
{
    /// <inheritdoc />
    public partial class updatedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubEvents",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoImageName",
                table: "EventOrganizers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageName",
                table: "EventOrganizers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubEvents",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LogoImageName",
                table: "EventOrganizers");

            migrationBuilder.DropColumn(
                name: "ProfileImageName",
                table: "EventOrganizers");
        }
    }
}
