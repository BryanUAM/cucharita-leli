using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucharitaLeliQR.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPremiosCanjeados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PremiosCanjeados",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiosCanjeados",
                table: "Clientes");
        }
    }
}
