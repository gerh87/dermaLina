using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dermasoft.core.data;

namespace Dermasoft.core.business
{
   public class RelVisitaMotivoFactory
    {
        public DermasoftEntities db = new DermasoftEntities();
        public int IdVisitaMotivo { get; set; }
        public int IdVisita { get; set; }
        public int? IdMotivoConsulta { get; set; }

        public virtual MotivoConsulta MotivoConsultas { get; set; }
        public virtual Visita Visitas { get; set; }

        public bool Guardar(RelVisitaMotivo model)
        {
            var factVisita = new VisitaFactory();
            var facMotivo = new MotivoConsultaFactory();
            if (factVisita.save(model.Visitas))
            {
                if (facMotivo.save(model.MotivoConsultas))
                {
                    if (saveRel())
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;

        }

        public bool saveRel()
        {
            var idVisita = db.Visita.Max(s => s.IdVisita);
            var idMotivo = db.MotivoConsulta.Max(s => s.IdMotivoConsulta);
            var modelo = new RelVisitaMotivo {IdVisita = idVisita, IdMotivoConsulta = idMotivo};
            db.Entry<RelVisitaMotivo>(modelo).State = EntityState.Added;
            db.SaveChanges();
            return true;
        }

     
    }
}
