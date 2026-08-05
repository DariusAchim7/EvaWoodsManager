using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaIstoricUtilizatorNoteInterneSiUrmatorulPas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriereUrmatorulPas",
                table: "Proiecte",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TermenUrmatorulPas",
                table: "Proiecte",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observatie",
                table: "IstoricStatusProiect",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Utilizator",
                table: "IstoricStatusProiect",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NoteInterneProiect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProiectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Autor = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    EsteFixata = table.Column<bool>(type: "boolean", nullable: false),
                    EsteRezolvata = table.Column<bool>(type: "boolean", nullable: false),
                    DataCreare = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataModificare = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteInterneProiect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteInterneProiect_Proiecte_ProiectId",
                        column: x => x.ProiectId,
                        principalTable: "Proiecte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteInterneProiect_ProiectId",
                table: "NoteInterneProiect",
                column: "ProiectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteInterneProiect");

            migrationBuilder.DropColumn(
                name: "DescriereUrmatorulPas",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "TermenUrmatorulPas",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "Observatie",
                table: "IstoricStatusProiect");

            migrationBuilder.DropColumn(
                name: "Utilizator",
                table: "IstoricStatusProiect");
        }
    }
}
