using Dermasoft.core.data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dermasoft.core.business
{
    public class UsuarioFactory
    {

        private DermasoftEntities db = new DermasoftEntities();
        public bool ExistUser(Usuario model)
        {
            return db.Usuario.FirstOrDefault(u => u.UsuarioNick == model.UsuarioNick && u.Contrasenia == model.Contrasenia) != null;
        }

        public List<Usuario> getUsuarios()
        {
            return db.Usuario.ToList();
        }

        public void saveUsuario(Usuario model)
        {
            if (model.IdUsuario == 0)
                db.Entry<Usuario>(model).State = EntityState.Added;
            else
                db.Entry<Usuario>(model).State = EntityState.Modified;
            db.SaveChanges();
        }

    }
}
