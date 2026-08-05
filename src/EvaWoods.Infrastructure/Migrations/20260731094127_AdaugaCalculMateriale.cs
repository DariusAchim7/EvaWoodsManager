using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaCalculMateriale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculeMateriale",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProiectId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialProdusId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumarTipuriPiese = table.Column<int>(type: "integer", nullable: false),
                    NumarBucati = table.Column<int>(type: "integer", nullable: false),
                    SuprafataPieseM2 = table.Column<decimal>(type: "numeric(12,4)", nullable: false),
                    FoiTeoretice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    FoiEstimate = table.Column<int>(type: "integer", nullable: false),
                    UtilizareProcent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    PierdereProcent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    LayoutJson = table.Column<string>(type: "text", nullable: true),
                    Erori = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DataCalcul = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculeMateriale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculeMateriale_Produse_MaterialProdusId",
                        column: x => x.MaterialProdusId,
                        principalTable: "Produse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalculeMateriale_Proiecte_ProiectId",
                        column: x => x.ProiectId,
                        principalTable: "Proiecte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculeMateriale_MaterialProdusId",
                table: "CalculeMateriale",
                column: "MaterialProdusId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculeMateriale_ProiectId_MaterialProdusId",
                table: "CalculeMateriale",
                columns: new[] { "ProiectId", "MaterialProdusId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculeMateriale");
        }
    }
}
