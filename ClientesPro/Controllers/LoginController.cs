using ClientesPro.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientesPro.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet("/login")]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginMetodo(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return BadRequest(new { success = false, errors = GetModelErrors() });

                return View(model);
            }

            // 🔹 Validación de credenciales (solo ejemplo)
            if (model.Email == "admin@correo.com" && model.Password == "123456")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Administrador"),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = false, // 🔸 No recuerda sesión
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                    });

                var redirectUrl = Url.Action("Index", "Home") ?? "/";

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Ok(new { success = true, redirect = redirectUrl });

                return Redirect(redirectUrl);
            }

            ModelState.AddModelError("", "Correo o contraseña incorrectos.");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return BadRequest(new { success = false, errors = GetModelErrors() });

            return View(model);
        }

        [HttpPost("/logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private Dictionary<string, string[]> GetModelErrors()
        {
            return ModelState
                .Where(ms => ms.Value?.Errors?.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
