//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dermasoft.core.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string Usuario1 { get; set; }
        public string Contrasenia { get; set; }
        public bool Activo { get; set; }
    
        public virtual Rol Role { get; set; }
    }
}