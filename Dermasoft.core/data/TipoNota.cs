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
    
    public partial class TipoNota
    {
        public TipoNota()
        {
            this.Notas = new HashSet<Nota>();
        }
    
        public int idTipoNota { get; set; }
        public string Descripcion { get; set; }
    
        public virtual ICollection<Nota> Notas { get; set; }
    }
}