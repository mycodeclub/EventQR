using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventQR.Migrations
{
    /// <inheritdoc />
    public partial class AddedSubEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Events_EventUniqueId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventUniqueId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventUniqueId",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "SubEvents",
                columns: table => new
                {
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubEventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubEvents", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_SubEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "UniqueId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubEvents_EventId",
                table: "SubEvents",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubEvents");

            migrationBuilder.AddColumn<Guid>(
                name: "EventUniqueId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventUniqueId",
                table: "Events",
                column: "EventUniqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Events_EventUniqueId",
                table: "Events",
                column: "EventUniqueId",
                principalTable: "Events",
                principalColumn: "UniqueId");
        }
    }
}
