using System.ComponentModel.DataAnnotations;

namespace ClientesPro.ViewModels
{
    public class LoginVM 
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [EmailAddress(ErrorMessage = "usuario no válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
