using ClientesPro.Models;
using Microsoft.EntityFrameworkCore;
namespace ClientesPro.Data
{
    public class ApplicationDbContext : DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : base(opciones) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region --> Tabla Clientes
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { Id = 1, NombreCompleto = "Ana Gómez Hernández", Direccion = "Av. Morelos #123" },
                new Cliente { Id = 2, NombreCompleto = "Luis Pérez Ortiz", Direccion = "Calle Hidalgo #456" },
                new Cliente { Id = 3, NombreCompleto = "María López Sánchez", Direccion = "Col. Centro #789" },
                new Cliente { Id = 4, NombreCompleto = "Carlos Ramírez Morales", Direccion = "Av. Reforma #234" },
                new Cliente { Id = 5, NombreCompleto = "Sofía Torres Díaz", Direccion = "Calle Juárez #567" }
            );
            #endregion

            #region --> Tabla Productos
            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = 1, Nombre = "Manzanas Rojas", Descripcion = "Manzanas frescas", Precio = 45.00m },
                new Producto { Id = 2, Nombre = "Arroz Integral", Descripcion = "Arroz integral de grano largo, alto en fibra y sin gluten.", Precio = 28.50m },
                new Producto { Id = 3, Nombre = "Leche Entera", Descripcion = "Leche entera pasteurizada de vaca, rica en calcio y proteínas.", Precio = 22.00m },
                new Producto { Id = 4, Nombre = "Pan Integral", Descripcion = "Pan integral artesanal con semillas y harina de trigo 100%.", Precio = 38.00m },
                new Producto { Id = 5, Nombre = "Huevos Orgánicos", Descripcion = "Huevos provenientes de gallinas libres de jaula, alimentadas naturalmente.", Precio = 52.00m }
            );
            #endregion

            #region --> Tabla Compras
            modelBuilder.Entity<Compra>().HasData(
                new Compra { Id = 1, ClienteId = 1, ProductoId = 3, Cantidad = 2, Fecha = DateTime.UtcNow, Total = 44.00m, Estado = "pagado" },
                new Compra { Id = 2, ClienteId = 2, ProductoId = 1, Cantidad = 1, Fecha = DateTime.UtcNow, Total = 45.00m, Estado = "pendiente" },
                new Compra { Id = 3, ClienteId = 3, ProductoId = 5, Cantidad = 3, Fecha = DateTime.UtcNow, Total = 156.00m, Estado = "pagado" },
                new Compra { Id = 4, ClienteId = 4, ProductoId = 4, Cantidad = 2, Fecha = DateTime.UtcNow, Total = 76.00m, Estado = "cancelado" },
                new Compra { Id = 5, ClienteId = 5, ProductoId = 2, Cantidad = 5, Fecha = DateTime.UtcNow, Total = 142.50m, Estado = "pagado" }
            );
            #endregion

        }
    }
}
