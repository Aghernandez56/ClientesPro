namespace ClientesPro.ViewModels
{
    public class CompraVM
    {
        public int Id { get; set; }

        // Cliente
        public int ClienteId { get; set; }
        public string? ClienteNombre { get; set; }

        // Producto
        public int ProductoId { get; set; }
        public string? ProductoNombre { get; set; }

        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string? Estado { get; set; }
    }
}
