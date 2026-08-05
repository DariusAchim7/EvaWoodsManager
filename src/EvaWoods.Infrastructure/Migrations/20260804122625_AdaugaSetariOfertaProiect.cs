using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaSetariOfertaProiect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AvansProcent",
                table: "Proiecte",
                type: "numeric(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TermenExecutieZile",
                table: "Proiecte",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValabilitateOfertaZile",
                table: "Proiecte",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvansProcent",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "TermenExecutieZile",
                table: "Proiecte");

            migrationBuilder.DropColumn(
                name: "ValabilitateOfertaZile",
                table: "Proiecte");
        }
    }
}
