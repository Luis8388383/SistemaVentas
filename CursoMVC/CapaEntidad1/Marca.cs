using System;

namespace CapaEntidad1
{
    public class Marca
    {
        public int IdMarca { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } // Cierre de propiedad corregido
    }
}



