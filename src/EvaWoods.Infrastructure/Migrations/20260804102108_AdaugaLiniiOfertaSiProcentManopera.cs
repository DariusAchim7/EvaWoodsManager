using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaLiniiOfertaSiProcentManopera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProcentManopera",
                table: "Proiecte",
                type: "numeric(5,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LiniiOferta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProiectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdusId = table.Column<Guid>(type: "uuid", nullable: false),
                    DescriereSnapshot = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CategorieSnapshot = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    UMSnapshot = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CantitateCalculata = table.Column<decimal>(type: "numeric(12,3)", nullable: false),
                    CantitateOfertata = table.Column<decimal>(type: "numeric(12,3)", nullable: false),
                    PretCostSnapshot = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    PretVanzareSnapshot = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    VizibilClient = table.Column<bool>(type: "boolean", nullable: false),
                    EsteAutomat = table.Column<bool>(type: "boolean", nullable: false),
                    Ordine = table.Column<int>(type: "integer", nullable: false),
                    DataAdaugare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiniiOferta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiniiOferta_Produse_ProdusId",
                        column: x => x.ProdusId,
                        principalTable: "Produse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LiniiOferta_Proiecte_ProiectId",
                        column: x => x.ProiectId,
                        principalTable: "Proiecte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiniiOferta_ProdusId",
                table: "LiniiOferta",
                column: "ProdusId");

            migrationBuilder.CreateIndex(
                name: "IX_LiniiOferta_ProiectId",
                table: "LiniiOferta",
                column: "ProiectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiniiOferta");

            migrationBuilder.DropColumn(
                name: "ProcentManopera",
                table: "Proiecte");
        }
    }
}
