using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaObservatiiDateMontajSiSpatii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccesAuto",
                table: "Proiecte",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AreLift",
                table: "Proiecte",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Etaj",
                table: "Proiecte",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntervalOrarPreferat",
                table: "Proiecte",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocParcare",
                table: "Proiecte",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservatiiClient",
                table: "Proiecte",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservatiiTransport",
                table: "Proiecte",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersoanaContactMontaj",
                table: "Proiecte",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpatiiProiect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProiectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nume = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Suprafata = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    InaltimeMm = table.Column<int>(type: "integer", nullable: true),
                    Orientare = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TipSpatiu = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StarePereti = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StarePardoseala = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataCreare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpatiiProiect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpatiiProiect_Proiecte_ProiectId",
                        column: x => x.ProiectId,
                        principalTable: "Proiecte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpatiiProiect_ProiectId",
                table: "SpatiiProiect",
                column: "ProiectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpatiiProiect");

            migrationBuilder.DropColumn(
                name: "AccesAuto",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "AreLift",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "Etaj",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "IntervalOrarPreferat",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "LocParcare",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "ObservatiiClient",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "ObservatiiTransport",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "PersoanaContactMontaj",
                table: "Proiecte");
        }
    }
}
