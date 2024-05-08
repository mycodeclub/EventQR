using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventQR.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QrScannAccount");

            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    UniqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserLoginId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScannerLoginId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_CheckIns_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "UniqueId");
                    table.ForeignKey(
                        name: "FK_CheckIns_Guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "UniqueId");
                    table.ForeignKey(
                        name: "FK_CheckIns_SubEvents_SubEventId",
                        column: x => x.SubEventId,
                        principalTable: "SubEvents",
                        principalColumn: "UniqueId");
                    table.ForeignKey(
                        name: "FK_CheckIns_TicketScanners_ScannerLoginId",
                        column: x => x.ScannerLoginId,
                        principalTable: "TicketScanners",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_EventId",
                table: "CheckIns",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_GuestId",
                table: "CheckIns",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_ScannerLoginId",
                table: "CheckIns",
                column: "ScannerLoginId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_SubEventId",
                table: "CheckIns",
                column: "SubEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckIns");

            migrationBuilder.CreateTable(
                name: "QrScannAccount",
                columns: table => new
                {
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoggedInUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MobileNo1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrScannAccount", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_QrScannAccount_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "UniqueId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QrScannAccount_EventId",
                table: "QrScannAccount",
                column: "EventId");
        }
    }
}
