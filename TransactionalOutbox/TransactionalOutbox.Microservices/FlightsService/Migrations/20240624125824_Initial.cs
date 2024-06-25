using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightsService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartureAirport = table.Column<string>(type: "text", nullable: false),
                    ArrivalAirport = table.Column<string>(type: "text", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightUuid);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    OutboxUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ContentJson = table.Column<string>(type: "text", nullable: false),
                    OccuredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.OutboxUuid);
                });

            migrationBuilder.CreateTable(
                name: "BookedFlights",
                columns: table => new
                {
                    BookingUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfTickets = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedFlights", x => x.BookingUuid);
                    table.ForeignKey(
                        name: "FK_BookedFlights_Flights_FlightUuid",
                        column: x => x.FlightUuid,
                        principalTable: "Flights",
                        principalColumn: "FlightUuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookedFlights_FlightUuid",
                table: "BookedFlights",
                column: "FlightUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookedFlights");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
