using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreightLook.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RateSheets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProcessingMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateSheets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirlineId = table.Column<int>(type: "int", nullable: false),
                    OriginAirportId = table.Column<int>(type: "int", nullable: false),
                    DestinationAirportId = table.Column<int>(type: "int", nullable: false),
                    RateSheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MinimumCharge = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    NormalRate = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    Rate45 = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    Rate100 = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    Rate250 = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    Rate300 = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    Rate500 = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    Rate1000 = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    Fsc = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Ssc = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Xray = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Misc = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    AmsAwb = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    AmsHawb = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    AmsEawb = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    AmsEhawb = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rates_Airports_DestinationAirportId",
                        column: x => x.DestinationAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rates_Airports_OriginAirportId",
                        column: x => x.OriginAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rates_RateSheets_RateSheetId",
                        column: x => x.RateSheetId,
                        principalTable: "RateSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RateStaging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateSheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    RawText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirlineText = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    OriginCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DestinationCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ValidationMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateStaging", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RateStaging_RateSheets_RateSheetId",
                        column: x => x.RateSheetId,
                        principalTable: "RateSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_Name",
                table: "Airlines",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_Code",
                table: "Airports",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rates_AirlineId",
                table: "Rates",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_DestinationAirportId",
                table: "Rates",
                column: "DestinationAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_OriginAirportId",
                table: "Rates",
                column: "OriginAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_RateSheetId",
                table: "Rates",
                column: "RateSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_RateStaging_RateSheetId_RowNumber",
                table: "RateStaging",
                columns: new[] { "RateSheetId", "RowNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "RateStaging");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "RateSheets");
        }
    }
}
