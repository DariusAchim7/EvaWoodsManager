using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaCorpuriSiPiese : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorpuriProiect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProiectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nume = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DataCreare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorpuriProiect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorpuriProiect_Proiecte_ProiectId",
                        column: x => x.ProiectId,
                        principalTable: "Proiecte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Piese",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProiectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CorpId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nume = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    LungimeMm = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    LatimeMm = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Cantitate = table.Column<int>(type: "integer", nullable: false),
                    MaterialProdusId = table.Column<Guid>(type: "uuid", nullable: true),
                    GrosimeMm = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    CantLaturiLungi = table.Column<int>(type: "integer", nullable: false),
                    CantLaturiScurte = table.Column<int>(type: "integer", nullable: false),
                    MaterialCantProdusId = table.Column<Guid>(type: "uuid", nullable: true),
                    DirectieFibra = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PermiteRotire = table.Column<bool>(type: "boolean", nullable: false),
                    Observatii = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataCreare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piese", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Piese_CorpuriProiect_CorpId",
                        column: x => x.CorpId,
                        principalTable: "CorpuriProiect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Piese_Produse_MaterialCantProdusId",
                        column: x => x.MaterialCantProdusId,
                        principalTable: "Produse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Piese_Produse_MaterialProdusId",
                        column: x => x.MaterialProdusId,
                        principalTable: "Produse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Piese_Proiecte_ProiectId",
                        column: x => x.ProiectId,
                        principalTable: "Proiecte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorpuriProiect_ProiectId",
                table: "CorpuriProiect",
                column: "ProiectId");

            migrationBuilder.CreateIndex(
                name: "IX_Piese_CorpId",
                table: "Piese",
                column: "CorpId");

            migrationBuilder.CreateIndex(
                name: "IX_Piese_MaterialCantProdusId",
                table: "Piese",
                column: "MaterialCantProdusId");

            migrationBuilder.CreateIndex(
                name: "IX_Piese_MaterialProdusId",
                table: "Piese",
                column: "MaterialProdusId");

            migrationBuilder.CreateIndex(
                name: "IX_Piese_ProiectId",
                table: "Piese",
                column: "ProiectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Piese");

            migrationBuilder.DropTable(
                name: "CorpuriProiect");
        }
    }
}
