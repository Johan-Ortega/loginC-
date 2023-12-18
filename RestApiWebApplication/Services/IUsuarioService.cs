using Microsoft.EntityFrameworkCore;
using RestApiWebApplication.Models;

namespace RestApiWebApplication.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string correo, string password);
        Task<Usuario> SaveUsuario(Usuario modelo);

    }
}
