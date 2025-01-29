using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarritoCompras.Api.compartidos.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "carrito_compras",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    codigo = table.Column<string>(type: "TEXT", nullable: false),
                    usuario_id = table.Column<string>(type: "TEXT", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    creado_por = table.Column<string>(type: "TEXT", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    modificado_por = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_carrito_compras", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "elementos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    codigo = table.Column<string>(type: "TEXT", nullable: false),
                    url_imagen = table.Column<string>(type: "TEXT", nullable: false),
                    cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    precio = table.Column<decimal>(type: "TEXT", nullable: false),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    carrito_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    creado_por = table.Column<string>(type: "TEXT", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    modificado_por = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_elementos", x => x.id);
                    table.ForeignKey(
                        name: "fk_elementos_carritos_carrito_id",
                        column: x => x.carrito_id,
                        principalTable: "carrito_compras",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_elementos_carrito_id",
                table: "elementos",
                column: "carrito_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "elementos");

            migrationBuilder.DropTable(
                name: "carrito_compras");
        }
    }
}
