using ClientesPro.Data;
using ClientesPro.Models;
using ClientesPro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesPro.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Compras()
        {
            var compras = (from c in _context.Compras
                           join cli in _context.Clientes on c.ClienteId equals cli.Id
                           join p in _context.Productos on c.ProductoId equals p.Id
                           orderby c.Fecha descending
                           select new CompraVM
                           {
                               Id = c.Id,
                               ClienteId = cli.Id,
                               ClienteNombre = cli.NombreCompleto,
                               ProductoId = p.Id,
                               ProductoNombre = p.Nombre,
                               Cantidad = c.Cantidad,
                               Fecha = c.Fecha,
                               Total = c.Total,
                               Estado = c.Estado
                           }).ToList();

            return View(compras);
        }

        /// <summary>
        /// Crea una nueva compra a partir de los datos enviados por AJAX.
        /// Valida el modelo, verifica existencia de cliente y producto, calcula el total,
        /// guarda la compra en la base de datos y devuelve un resumen de la compra creada.
        /// </summary>
        /// <param name="vm">Datos de la compra enviados desde el cliente</param>
        /// <param name="ct">Token de cancelación para operaciones asincrónicas</param>
        /// <returns>Json con estado de éxito o error, y datos de la compra si fue exitosa</returns>
        [HttpPost]
        public async Task<JsonResult> CrearAjax([FromBody] CompraVM vm, CancellationToken ct)
        {
            // 1) Validación del modelo usando DataAnnotations en CompraVM
            if (!ModelState.IsValid)
            {
                // Extrae todos los mensajes de error de validación
                var errores = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();

                return Json(new { exito = false, mensaje = "Modelo inválido.", errores });
            }

            // 2) Validación rápida: cantidad mínima
            if (vm.Cantidad < 1)
                return Json(new { exito = false, mensaje = "La cantidad debe ser mínimo 1." });

            try
            {
                // 3) Búsqueda paralela de cliente y producto para mejorar rendimiento
                var clienteTask = _context.Clientes.FindAsync(new object[] { vm.ClienteId }, ct).AsTask();
                var productoTask = _context.Productos.FindAsync(new object[] { vm.ProductoId }, ct).AsTask();

                await Task.WhenAll(clienteTask, productoTask);

                var cliente = clienteTask.Result;
                var producto = productoTask.Result;

                // Validación de existencia de cliente
                if (cliente == null)
                    return Json(new { exito = false, mensaje = "Cliente no encontrado." });

                // Validación de existencia de producto
                if (producto == null)
                    return Json(new { exito = false, mensaje = "Producto no encontrado." });

                // 4) Determinación de fecha y cálculo de total
                var fecha = vm.Fecha == default ? DateTime.UtcNow : vm.Fecha;
                decimal total = producto.Precio * vm.Cantidad;

                // 5) Creación de la entidad Compra
                var compra = new Compra
                {
                    ClienteId = vm.ClienteId,
                    ProductoId = vm.ProductoId,
                    Cantidad = vm.Cantidad,
                    Fecha = fecha,
                    Total = total,
                    Estado = string.IsNullOrWhiteSpace(vm.Estado) ? "pendiente" : vm.Estado.Trim()
                };

                // Guardado en base de datos
                _context.Compras.Add(compra);
                await _context.SaveChangesAsync(ct);

                // 6) Construcción del objeto de respuesta con datos 
                var compraCreadaVm = new CompraVM
                {
                    Id = compra.Id,
                    ClienteId = cliente.Id,
                    ClienteNombre = cliente.NombreCompleto,
                    ProductoId = producto.Id,
                    ProductoNombre = producto.Nombre,
                    Cantidad = compra.Cantidad,
                    Total = compra.Total,
                    Fecha = compra.Fecha,
                    Estado = compra.Estado
                };

                return Json(new { exito = true, datos = compraCreadaVm });
            }
            catch (OperationCanceledException)
            {
                // Cancelación explícita de la operación (por timeout o usuario)
                return Json(new { exito = false, mensaje = "Operación cancelada." });
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores inesperados
                return Json(new { exito = false, mensaje = "Ocurrió un error en el servidor.", detalle = ex.Message });
            }
        }


    }
}
