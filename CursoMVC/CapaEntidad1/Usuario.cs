using System;

namespace CapaEntidad1
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public bool Reestablecer { get; set; } = true;
        public bool Activo { get; set; } = true;
    }
}
