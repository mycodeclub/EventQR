using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventQR.Migrations
{
    /// <inheritdoc />
    public partial class TicketScannerMig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketScanners_Events_EventId",
                table: "TicketScanners");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserLoginId",
                table: "TicketScanners",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "TicketScanners",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "TicketScanners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mobile1",
                table: "TicketScanners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mobile2",
                table: "TicketScanners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TicketScanners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketScanners_Events_EventId",
                table: "TicketScanners",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "UniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketScanners_Events_EventId",
                table: "TicketScanners");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "TicketScanners");

            migrationBuilder.DropColumn(
                name: "Mobile1",
                table: "TicketScanners");

            migrationBuilder.DropColumn(
                name: "Mobile2",
                table: "TicketScanners");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TicketScanners");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserLoginId",
                table: "TicketScanners",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "TicketScanners",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketScanners_Events_EventId",
                table: "TicketScanners",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
