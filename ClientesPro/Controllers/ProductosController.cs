using ClientesPro.Data;
using ClientesPro.Models;
using ClientesPro.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClientesPro.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Productos()
        {
            var productos = _context.Productos
                .OrderBy(p => p.Nombre)
                .Select(p => new ProductoVM
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio
                })
                .ToList();

            return View(productos);
        }

        [HttpPost]
        public async Task<JsonResult> CrearAjax([FromBody] ProductoVM vm, CancellationToken ct)
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
                var nuevo = new Producto
                {
                    Nombre = vm.Nombre?.Trim(),
                    Descripcion = vm.Descripcion?.Trim(),
                    Precio = vm.Precio
                };

                _context.Productos.Add(nuevo);
                await _context.SaveChangesAsync(ct);

                var creadoVm = new ProductoVM
                {
                    Id = nuevo.Id,
                    Nombre = nuevo.Nombre,
                    Descripcion = nuevo.Descripcion,
                    Precio = nuevo.Precio
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
