using System.ComponentModel.DataAnnotations;

namespace ClientesPro.ViewModels
{
    public class ClienteVM
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string? NombreCompleto { get; set; }

        public string? Direccion { get; set; }
    }
}
