using System.ComponentModel.DataAnnotations;

namespace ClientesPro.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string? NombreCompleto { get; set; }

        public string? Direccion { get; set; }

    }
}
