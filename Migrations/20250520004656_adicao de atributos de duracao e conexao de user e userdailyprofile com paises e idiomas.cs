using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LumeServer.Migrations
{
    /// <inheritdoc />
    public partial class adicaodeatributosdeduracaoeconexaodeusereuserdailyprofilecompaiseseidiomas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxDuration",
                table: "UserDailyProfiles",
                type: "int",
                nullable: false,
                defaultValue: 1000);

            migrationBuilder.AddColumn<int>(
                name: "MinDuration",
                table: "UserDailyProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxDuration",
                table: "ExtraAnswers",
                type: "int",
                nullable: false,
                defaultValue: 1000);

            migrationBuilder.AddColumn<int>(
                name: "MinDuration",
                table: "ExtraAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxDuration",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 1000);

            migrationBuilder.AddColumn<int>(
                name: "MinDuration",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserDailyProfileProductionCountries",
                columns: table => new
                {
                    UserDailyProfileId = table.Column<int>(type: "int", nullable: false),
                    ProductionCountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDailyProfileProductionCountries", x => new { x.ProductionCountryId, x.UserDailyProfileId });
                    table.ForeignKey(
                        name: "FK_UserDailyProfileProductionCountries_ProductionCountries_Prod~",
                        column: x => x.ProductionCountryId,
                        principalTable: "ProductionCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDailyProfileProductionCountries_UserDailyProfiles_UserDa~",
                        column: x => x.UserDailyProfileId,
                        principalTable: "UserDailyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserDailyProfileSpokenLanguages",
                columns: table => new
                {
                    UserDailyProfileId = table.Column<int>(type: "int", nullable: false),
                    SpokenLanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDailyProfileSpokenLanguages", x => new { x.SpokenLanguageId, x.UserDailyProfileId });
                    table.ForeignKey(
                        name: "FK_UserDailyProfileSpokenLanguages_SpokenLanguages_SpokenLangua~",
                        column: x => x.SpokenLanguageId,
                        principalTable: "SpokenLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDailyProfileSpokenLanguages_UserDailyProfiles_UserDailyP~",
                        column: x => x.UserDailyProfileId,
                        principalTable: "UserDailyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGeneralProfileProductionCountries",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductionCountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGeneralProfileProductionCountries", x => new { x.ProductionCountryId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserGeneralProfileProductionCountries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGeneralProfileProductionCountries_ProductionCountries_Pr~",
                        column: x => x.ProductionCountryId,
                        principalTable: "ProductionCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGeneralProfileSpokenLanguages",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpokenLanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGeneralProfileSpokenLanguages", x => new { x.SpokenLanguageId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserGeneralProfileSpokenLanguages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGeneralProfileSpokenLanguages_SpokenLanguages_SpokenLang~",
                        column: x => x.SpokenLanguageId,
                        principalTable: "SpokenLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserDailyProfileProductionCountries_UserDailyProfileId",
                table: "UserDailyProfileProductionCountries",
                column: "UserDailyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDailyProfileSpokenLanguages_UserDailyProfileId",
                table: "UserDailyProfileSpokenLanguages",
                column: "UserDailyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGeneralProfileProductionCountries_UserId",
                table: "UserGeneralProfileProductionCountries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGeneralProfileSpokenLanguages_UserId",
                table: "UserGeneralProfileSpokenLanguages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDailyProfileProductionCountries");

            migrationBuilder.DropTable(
                name: "UserDailyProfileSpokenLanguages");

            migrationBuilder.DropTable(
                name: "UserGeneralProfileProductionCountries");

            migrationBuilder.DropTable(
                name: "UserGeneralProfileSpokenLanguages");

            migrationBuilder.DropColumn(
                name: "MaxDuration",
                table: "UserDailyProfiles");

            migrationBuilder.DropColumn(
                name: "MinDuration",
                table: "UserDailyProfiles");

            migrationBuilder.DropColumn(
                name: "MaxDuration",
                table: "ExtraAnswers");

            migrationBuilder.DropColumn(
                name: "MinDuration",
                table: "ExtraAnswers");

            migrationBuilder.DropColumn(
                name: "MaxDuration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MinDuration",
                table: "AspNetUsers");
        }
    }
}
