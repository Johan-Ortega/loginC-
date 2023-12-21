using Microsoft.AspNetCore.Mvc;

using RestApiWebApplication.Models;
using RestApiWebApplication.Resources;
using RestApiWebApplication.Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace RestApiWebApplication.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;
        public InicioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Password = Utilidades.EncriptarClave(modelo.Password);

             Usuario usuarioExistente = await _usuarioServicio.GetUsuario(modelo.Correo, modelo.Password);

            if (usuarioExistente != null)
            {
                ViewData["Mensaje"] = "Ya existe un usuario con este correo electrónico";
                return View();
            }

            Usuario usuario_creado = await _usuarioServicio.SaveUsuario(modelo);

            if (usuario_creado.IdUsuario > 0){
                // return RedirectToAction("IniciarSesion", "Inicio");
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, usuario_creado.NombreUsuario),
                    new Claim("Apellido",usuario_creado.ApellidoUsuario),
                    new Claim("Correo", usuario_creado.Correo),
                    new Claim("Password", usuario_creado.Password),
                    new Claim("Ubicacion", usuario_creado.Ubicacion)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh= true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                    );

                return RedirectToAction("Index", "System");
            }else{
                ViewData["Mensaje"] = "No se pudo crear el usuario";
                return View();
            }
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string password)
        {

            Usuario usuario_encontrado = await _usuarioServicio.GetUsuario(correo, Utilidades.EncriptarClave(password));

            if (usuario_encontrado == null) {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario),
                new Claim("Apellido",usuario_encontrado.ApellidoUsuario),
                new Claim("Correo", usuario_encontrado.Correo),
                new Claim("Password", usuario_encontrado.Password),
                new Claim("Ubicacion", usuario_encontrado.Ubicacion)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh= true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "System");
        }
    }
}