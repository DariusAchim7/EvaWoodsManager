using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtindeProduse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Cod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nume = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Categorie = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Subcategorie = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    UM = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PretAchizitie = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    PretCalcul = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    ProcentTva = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    FurnizorId = table.Column<Guid>(type: "uuid", nullable: true),
                    EsteActiv = table.Column<bool>(type: "boolean", nullable: false),
                    DataUltimeiActualizariPret = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observatii = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DataCreare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LungimeFoaieMm = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    LatimeFoaieMm = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    GrosimeMm = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    PermiteRotire = table.Column<bool>(type: "boolean", nullable: false),
                    KerfMm = table.Column<decimal>(type: "numeric(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produse_Furnizori_FurnizorId",
                        column: x => x.FurnizorId,
                        principalTable: "Furnizori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produse_Cod",
                table: "Produse",
                column: "Cod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produse_FurnizorId",
                table: "Produse",
                column: "FurnizorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produse");
        }
    }
}
