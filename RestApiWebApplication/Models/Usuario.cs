using System;
using System.Collections.Generic;

namespace RestApiWebApplication.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? ApellidoUsuario { get; set; }

    public string? Correo { get; set; }

    public string? Password { get; set; }

    public string? Ubicacion { get; set; }

}
