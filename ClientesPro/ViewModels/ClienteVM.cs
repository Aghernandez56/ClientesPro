using System.ComponentModel.DataAnnotations;

namespace ClientesPro.ViewModels
{
    public class ClienteVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede exceder {1} caracteres.")]
        [Display(Name = "Nombre completo")]
        public string? NombreCompleto { get; set; }

        [MaxLength(250, ErrorMessage = "La dirección no puede exceder {1} caracteres.")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }
    }
}
