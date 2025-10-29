using System.ComponentModel.DataAnnotations;

namespace ClientesPro.ViewModels
{
    public class ProductoVM
    {
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }
    }
}
