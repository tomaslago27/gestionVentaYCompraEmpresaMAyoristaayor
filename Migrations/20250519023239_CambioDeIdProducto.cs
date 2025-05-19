using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaVentasAPI.Migrations
{
    /// <inheritdoc />
    public partial class CambioDeIdProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_Compra",
                table: "DetallesCompra");

            migrationBuilder.DropColumn(
                name: "Id_Producto",
                table: "DetallesCompra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Compra",
                table: "DetallesCompra",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Producto",
                table: "DetallesCompra",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
