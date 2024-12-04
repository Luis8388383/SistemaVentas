using System;

namespace CapaEntidad1
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } // Cierre de propiedad corregido
    }
}
