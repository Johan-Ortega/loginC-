using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiWebApplication.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace RestApiWebApplication.Controllers
{
    [Authorize]
    public class SystemController : Controller
    {
        private readonly ILogger<SystemController> _logger;

      
        public SystemController(ILogger<SystemController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";
            string apellidoUsuario = "";
            string correoUsuario = "";
            string passwordUsuario = "";
            string ubicacionUsuario = "";

            if (claimuser.Identity.IsAuthenticated) {
                nombreUsuario  = claimuser.Claims.Where(c=> c.Type == ClaimTypes.Name)
                    .Select(c=>c.Value).SingleOrDefault();
                apellidoUsuario = claimuser.Claims.Where(c => c.Type == "Apellido")
                    .Select(c => c.Value).SingleOrDefault();
                correoUsuario = claimuser.Claims.Where(c => c.Type == "Correo")
                    .Select(c => c.Value).SingleOrDefault();
                passwordUsuario = claimuser.Claims.Where(c => c.Type == "Password")
                    .Select(c => c.Value).SingleOrDefault();
                ubicacionUsuario = claimuser.Claims.Where(c => c.Type == "Ubicacion")
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["apellidoUsuario"] = apellidoUsuario;
            ViewData["correoUsuario"] = correoUsuario;
            ViewData["passwordUsuario"] = passwordUsuario;
            ViewData["ubicacionUsuario"] = ubicacionUsuario;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task< IActionResult >CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IniciarSesion", "Inicio");
        }
    }
}