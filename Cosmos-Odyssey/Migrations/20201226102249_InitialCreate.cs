using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cosmos_Odyssey.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemandingCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandingCustomer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "From",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_From", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pricelist",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ready = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "To",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_To", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Distance = table.Column<long>(type: "bigint", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteInfo_From_FromId",
                        column: x => x.FromId,
                        principalTable: "From",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteInfo_To_ToId",
                        column: x => x.ToId,
                        principalTable: "To",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leg",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PriceListId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RouteInfoId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leg_PriceList_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "Pricelist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leg_RouteInfo_RouteInfoId",
                        column: x => x.RouteInfoId,
                        principalTable: "RouteInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    FlightStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LegId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provider_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Provider_Leg_LegId",
                        column: x => x.LegId,
                        principalTable: "Leg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DemandingCustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_DemandingCustomer_DemandingCustomerId",
                        column: x => x.DemandingCustomerId,
                        principalTable: "DemandingCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Provider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leg_PriceListId",
                table: "Leg",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_Leg_RouteInfoId",
                table: "Leg",
                column: "RouteInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_CompanyId",
                table: "Provider",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_LegId",
                table: "Provider",
                column: "LegId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_DemandingCustomerId",
                table: "Reservation",
                column: "DemandingCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ProviderId",
                table: "Reservation",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteInfo_FromId",
                table: "RouteInfo",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteInfo_ToId",
                table: "RouteInfo",
                column: "ToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "DemandingCustomer");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Leg");

            migrationBuilder.DropTable(
                name: "Pricelist");

            migrationBuilder.DropTable(
                name: "RouteInfo");

            migrationBuilder.DropTable(
                name: "From");

            migrationBuilder.DropTable(
                name: "To");
        }
    }
}
