using System.ComponentModel.DataAnnotations;

namespace ClientesPro.ViewModels
{


    public class CompraVM
    {
        public int Id { get; set; }

        // Cliente
        [Required(ErrorMessage = "Selecciona un cliente.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public string? ClienteNombre { get; set; }

        // Producto
        [Required(ErrorMessage = "Selecciona un producto.")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }
        public string? ProductoNombre { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mínimo 1.")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; } = 1;

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; }


    }
}
