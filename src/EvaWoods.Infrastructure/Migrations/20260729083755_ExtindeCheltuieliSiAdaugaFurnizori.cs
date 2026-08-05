using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtindeCheltuieliSiAdaugaFurnizori : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BugetAlocat",
                table: "Proiecte",
                type: "numeric(12,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Furnizori",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nume = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Furnizori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cheltuieli",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProiectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descriere = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Categorie = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Subcategorie = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FurnizorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Data = table.Column<DateTime>(type: "date", nullable: false),
                    NumarFactura = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ValoareFaraTva = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    ProcentTva = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Tva = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    MetodaPlata = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    StatusPlata = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IncludeInBuget = table.Column<bool>(type: "boolean", nullable: false),
                    Observatii = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataCreare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheltuieli", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cheltuieli_Furnizori_FurnizorId",
                        column: x => x.FurnizorId,
                        principalTable: "Furnizori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Cheltuieli_Proiecte_ProiectId",
                        column: x => x.ProiectId,
                        principalTable: "Proiecte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cheltuieli_FurnizorId",
                table: "Cheltuieli",
                column: "FurnizorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheltuieli_ProiectId",
                table: "Cheltuieli",
                column: "ProiectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cheltuieli");

            migrationBuilder.DropTable(
                name: "Furnizori");

            migrationBuilder.DropColumn(
                name: "BugetAlocat",
                table: "Proiecte");
        }
    }
}
