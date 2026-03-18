using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucharitaLeliQR.Migrations
{
    /// <inheritdoc />
    public partial class AgregarQRCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoQR",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoQR",
                table: "Clientes");
        }
    }
}
