using ClientesPro.Data;
using ClientesPro.Models;
using ClientesPro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public IActionResult Buscar(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
            {
                // Puedes regresar todos o ninguno; aquí regresamos todos ordenados
                var todos = _context.Productos
                    .OrderBy(p => p.Nombre)
                    .Select(p => new ProductoVM
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Precio = p.Precio
                    })
                    .ToList();

                return Json(todos);
            }

            criterio = criterio.Trim().ToLower();

            var resultados = _context.Productos
                .Where(p => (p.Nombre != null && p.Nombre.ToLower().Contains(criterio))
                         || (p.Descripcion != null && p.Descripcion.ToLower().Contains(criterio)))
                .OrderBy(p => p.Nombre)
                .Select(p => new ProductoVM
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio
                })
                .ToList();

            return Json(resultados);
        }

        [HttpPost]
        public async Task<JsonResult> CrearAjax([FromBody] ProductoVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                // Diccionario campo => lista de mensajes
                var erroresPorCampo = ModelState
                    .Where(kvp => kvp.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { exito = false, mensaje = "Modelo inválido.", errores = erroresPorCampo });
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

        [HttpGet]
        public async Task<IActionResult> ActualizarProductos(int? id)
        {
            if (!id.HasValue || id<=0)
            {
                // Nuevo producto
                var vmNuevo = new ProductoVM();
                return View(vmNuevo);
            }

            var producto = await _context.Productos.FindAsync(id.Value);
            if (producto == null)
                return NotFound(); // o puedes devolver View(new ProductoVM()) si prefieres crear en blanco

            var vm = new ProductoVM
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio
            };

            return View(vm);
        }

        //public IActionResult ActualizarProductos()
        //{
          

        //    return View();
        //}

        //[HttpPut]
        //public async Task<bool> Actualizar(Producto pro)
        //{
        //    try
        //    {
        //        var producto = await _context.Productos.FindAsync(pro.Id);

        //        if (producto == null) return false;

        //        _context.Productos.Update(producto);

        //        await _context.SaveChangesAsync();

        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        [HttpDelete]
        public async Task<bool> Eliminar(int id) 
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null) return false;
            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();

            return true;

        }

    }
}
