using System.Collections.Generic;
using System.Linq;
using Dermasoft.core.data;

namespace Dermasoft.web.Models
{
    public class RolModel
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        private static readonly DermasoftEntities Db = new DermasoftEntities();


        public static List<RolModel> GetAll()
        {
            return Db.Rol.Select(x => new RolModel
            {
                IdRol = x.IdRol,
                Nombre = x.Nombre
            }).ToList();
        }

    }
}
