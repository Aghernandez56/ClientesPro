using ClientesPro.Data;
using ClientesPro.Models;
using ClientesPro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesPro.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Clientes()
        {
            var clientes = _context.Clientes
                .OrderBy(c => c.NombreCompleto)
                .Select(c => new ClienteVM
                {
                    Id = c.Id,
                    NombreCompleto = c.NombreCompleto,
                    Direccion = c.Direccion
                })
                .ToList();

            return View(clientes); 
        }

        [HttpPost]
        public async Task<JsonResult> CrearAjax([FromBody] ClienteVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();

                return Json(new { exito = false, mensaje = "Modelo inválido.", errores });
            }

            try
            {
                var nuevo = new Cliente
                {
                    NombreCompleto = vm.NombreCompleto?.Trim(),
                    Direccion = string.IsNullOrWhiteSpace(vm.Direccion) ? null : vm.Direccion.Trim()
                };

                _context.Clientes.Add(nuevo);
                await _context.SaveChangesAsync(ct);

                // Devolver el cliente creado como ClienteVM
                var creadoVm = new ClienteVM
                {
                    Id = nuevo.Id,
                    NombreCompleto = nuevo.NombreCompleto,
                    Direccion = nuevo.Direccion
                };

                return Json(new { exito = true, datos = creadoVm });
            }
            catch (OperationCanceledException)
            {
                return Json(new { exito = false, mensaje = "Operación cancelada." });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Ocurrió un error en el servidor.", detalle = ex.Message });
            }
        }

    }
}
