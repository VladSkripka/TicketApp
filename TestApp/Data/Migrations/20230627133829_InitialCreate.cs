using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApp.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    PublicID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FID = table.Column<int>(type: "int", nullable: false),
                    departure_point = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dep_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    arrival_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ticket_price = table.Column<float>(type: "real", nullable: false),
                    sites = table.Column<int>(type: "int", nullable: false),
                    sites_left = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.PublicID);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Public_ID = table.Column<int>(type: "int", nullable: false),
                    passenger_first = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passenger_last = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightPublicID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tickets_Flights_FlightPublicID",
                        column: x => x.FlightPublicID,
                        principalTable: "Flights",
                        principalColumn: "PublicID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightPublicID",
                table: "Tickets",
                column: "FlightPublicID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
