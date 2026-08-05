using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaWoods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaServiciuAsociatProdus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiciuAsociatId",
                table: "Produse",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produse_ServiciuAsociatId",
                table: "Produse",
                column: "ServiciuAsociatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produse_Produse_ServiciuAsociatId",
                table: "Produse",
                column: "ServiciuAsociatId",
                principalTable: "Produse",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produse_Produse_ServiciuAsociatId",
                table: "Produse");

            migrationBuilder.DropIndex(
                name: "IX_Produse_ServiciuAsociatId",
                table: "Produse");

            migrationBuilder.DropColumn(
                name: "ServiciuAsociatId",
                table: "Produse");
        }
    }
}
