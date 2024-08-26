using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventQR.Migrations
{
    /// <inheritdoc />
    public partial class ass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HostDetails",
                columns: table => new
                {
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HostOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HostTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumberOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumberTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HostOneDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HostTwoDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostDetails", x => x.UniqueId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HostDetails");
        }
    }
}
