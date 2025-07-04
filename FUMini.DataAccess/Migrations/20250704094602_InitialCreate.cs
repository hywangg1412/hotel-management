using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUMini.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerFullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerBirthday = table.Column<DateOnly>(type: "date", nullable: true),
                    CustomerStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "roomTypes",
                columns: table => new
                {
                    RoomTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TypeDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TypeNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roomTypes", x => x.RoomTypeID);
                });

            migrationBuilder.CreateTable(
                name: "BookingReservations",
                columns: table => new
                {
                    BookingReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BookingStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingReservations", x => x.BookingReservationID);
                    table.ForeignKey(
                        name: "FK_BookingReservations_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomInformations",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoomDetailDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomMaxCapacity = table.Column<int>(type: "int", nullable: true),
                    RoomStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    RoomPricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RoomTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomInformations", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_RoomInformations_roomTypes_RoomTypeID",
                        column: x => x.RoomTypeID,
                        principalTable: "roomTypes",
                        principalColumn: "RoomTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    BookingReservationID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ActualPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => new { x.BookingReservationID, x.RoomID });
                    table.ForeignKey(
                        name: "FK_BookingDetails_BookingReservations_BookingReservationID",
                        column: x => x.BookingReservationID,
                        principalTable: "BookingReservations",
                        principalColumn: "BookingReservationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDetails_RoomInformations_RoomID",
                        column: x => x.RoomID,
                        principalTable: "RoomInformations",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_RoomID",
                table: "BookingDetails",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingReservations_CustomerID",
                table: "BookingReservations",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomInformations_RoomTypeID",
                table: "RoomInformations",
                column: "RoomTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.DropTable(
                name: "BookingReservations");

            migrationBuilder.DropTable(
                name: "RoomInformations");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "roomTypes");
        }
    }
}
