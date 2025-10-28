using System.ComponentModel.DataAnnotations;

namespace ClientesPro.Models
{
    public class Compra
    {
        public int Id { get; set; }

        // FK Cliente
        [Required]
        public int ClienteId { get; set; }

        // FK Producto
        [Required]
        public int ProductoId { get; set; }

        public int Cantidad { get; set; } = 1;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public decimal Total { get; set; }

        // Estado opcional: "pendiente","pagado","cancelado"
        public string Estado { get; set; } = "pendiente";
    }
}
