using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaAdresaMontajSiIstoricStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdresaMontaj",
                table: "Proiecte",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IstoricStatusProiect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProiectId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusVechi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StatusNou = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DataSchimbare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IstoricStatusProiect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IstoricStatusProiect_Proiecte_ProiectId",
                        column: x => x.ProiectId,
                        principalTable: "Proiecte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IstoricStatusProiect_ProiectId",
                table: "IstoricStatusProiect",
                column: "ProiectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IstoricStatusProiect");

            migrationBuilder.DropColumn(
                name: "AdresaMontaj",
                table: "Proiecte");
        }
    }
}
