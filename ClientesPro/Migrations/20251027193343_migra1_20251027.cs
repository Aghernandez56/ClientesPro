using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClientesPro.Migrations
{
    /// <inheritdoc />
    public partial class migra1_20251027 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCompleto = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Direccion", "NombreCompleto" },
                values: new object[,]
                {
                    { 1, "Av. Morelos #123", "Ana Gómez Hernández" },
                    { 2, "Calle Hidalgo #456", "Luis Pérez Ortiz" },
                    { 3, "Col. Centro #789", "María López Sánchez" },
                    { 4, "Av. Reforma #234", "Carlos Ramírez Morales" },
                    { 5, "Calle Juárez #567", "Sofía Torres Díaz" }
                });

            migrationBuilder.InsertData(
                table: "Compras",
                columns: new[] { "Id", "Cantidad", "ClienteId", "Estado", "Fecha", "ProductoId", "Total" },
                values: new object[,]
                {
                    { 1, 2, 1, "pagado", new DateTime(2025, 10, 27, 19, 33, 43, 245, DateTimeKind.Utc).AddTicks(5282), 3, 44.00m },
                    { 2, 1, 2, "pendiente", new DateTime(2025, 10, 27, 19, 33, 43, 245, DateTimeKind.Utc).AddTicks(5285), 1, 45.00m },
                    { 3, 3, 3, "pagado", new DateTime(2025, 10, 27, 19, 33, 43, 245, DateTimeKind.Utc).AddTicks(5286), 5, 156.00m },
                    { 4, 2, 4, "cancelado", new DateTime(2025, 10, 27, 19, 33, 43, 245, DateTimeKind.Utc).AddTicks(5288), 4, 76.00m },
                    { 5, 5, 5, "pagado", new DateTime(2025, 10, 27, 19, 33, 43, 245, DateTimeKind.Utc).AddTicks(5289), 2, 142.50m }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Descripcion", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, "Manzanas frescas", "Manzanas Rojas", 45.00m },
                    { 2, "Arroz integral de grano largo, alto en fibra y sin gluten.", "Arroz Integral", 28.50m },
                    { 3, "Leche entera pasteurizada de vaca, rica en calcio y proteínas.", "Leche Entera", 22.00m },
                    { 4, "Pan integral artesanal con semillas y harina de trigo 100%.", "Pan Integral", 38.00m },
                    { 5, "Huevos provenientes de gallinas libres de jaula, alimentadas naturalmente.", "Huevos Orgánicos", 52.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
