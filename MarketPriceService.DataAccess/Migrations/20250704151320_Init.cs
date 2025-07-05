using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPriceService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstrumentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TickSize = table.Column<double>(type: "float", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultOrderSize = table.Column<int>(type: "int", nullable: false),
                    MaxOrderSize = table.Column<int>(type: "int", nullable: true),
                    InstrumentEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstrumentMappings_Instruments_InstrumentEntityId",
                        column: x => x.InstrumentEntityId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InstrumentEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstrumentProfiles_Instruments_InstrumentEntityId",
                        column: x => x.InstrumentEntityId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradingHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegularStart = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RegularEnd = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ElectronicStart = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ElectronicEnd = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradingHours_InstrumentMappings_Id",
                        column: x => x.Id,
                        principalTable: "InstrumentMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GicsInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    IndustryGroupId = table.Column<int>(type: "int", nullable: false),
                    IndustryId = table.Column<int>(type: "int", nullable: false),
                    SubIndustryId = table.Column<int>(type: "int", nullable: false),
                    InstrumentProfileEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GicsInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GicsInfos_InstrumentProfiles_InstrumentProfileEntityId",
                        column: x => x.InstrumentProfileEntityId,
                        principalTable: "InstrumentProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GicsInfos_InstrumentProfileEntityId",
                table: "GicsInfos",
                column: "InstrumentProfileEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentMappings_InstrumentEntityId",
                table: "InstrumentMappings",
                column: "InstrumentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentProfiles_InstrumentEntityId",
                table: "InstrumentProfiles",
                column: "InstrumentEntityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GicsInfos");

            migrationBuilder.DropTable(
                name: "TradingHours");

            migrationBuilder.DropTable(
                name: "InstrumentProfiles");

            migrationBuilder.DropTable(
                name: "InstrumentMappings");

            migrationBuilder.DropTable(
                name: "Instruments");
        }
    }
}
