using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaProiecte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proiecte",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    CodProiect = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nume = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipMobilier = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Valoare = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    TermenLimita = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Progres = table.Column<int>(type: "integer", nullable: false),
                    DataCreare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proiecte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proiecte_Clienti_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proiecte_ClientId",
                table: "Proiecte",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Proiecte_CodProiect",
                table: "Proiecte",
                column: "CodProiect",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proiecte");
        }
    }
}
