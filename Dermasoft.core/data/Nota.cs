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
    
    public partial class Nota
    {
        public int IdNota { get; set; }
        public int IdVisita { get; set; }
        public int IdTipoNota { get; set; }
        public string Descripcion { get; set; }
    
        public virtual TipoNota TipoNota { get; set; }
        public virtual Visita Visitas { get; set; }
        public virtual Visita Visitas1 { get; set; }
    }
}
