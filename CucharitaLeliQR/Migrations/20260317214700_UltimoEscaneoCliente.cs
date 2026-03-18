using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucharitaLeliQR.Migrations
{
    /// <inheritdoc />
    public partial class UltimoEscaneoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UltimoEscaneo",
                table: "Clientes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UltimoEscaneo",
                table: "Clientes");
        }
    }
}
