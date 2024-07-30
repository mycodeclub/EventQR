using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;

#nullable disable

namespace EventQR.Migrations
{
    /// <inheritdoc />
    public partial class HostDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventHostDeatils",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                    HostOne = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    HostTwo = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ContactNumberOne = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    ContactNumberTwo = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    HostOneDesignation = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    HostTwoDesignation = table.Column<string>(type: "nvarchar(100)", nullable: true)

                },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_HostDeatilsId", x => x.UniqueId);
                 });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name : "EventHostDeatils");

        }
    }
}
