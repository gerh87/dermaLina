using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dermasoft.core.data;

namespace Dermasoft.core.business
{
    public class MotivoConsultaFactory
    {
        public DermasoftEntities db = new DermasoftEntities();

        public MotivoConsultaFactory()
        {
            this.RelVisitaMotivo = new HashSet<RelVisitaMotivo>();
       

        }

        public int IdMotivoConsulta { get; set; }
        public string MotivoConsulta { get; set; }
        public string AntecedentesEA { get; set; }

        public bool? Activo { get; set; }
      
        public virtual ICollection<RelVisitaMotivo> RelVisitaMotivo { get; set; }

        public bool save(MotivoConsulta modelo)
        {
            var model = new MotivoConsulta {IdMotivoConsulta = modelo.IdMotivoConsulta, MotivoConsulta = modelo.MotivoConsulta, AntecedentesEA = modelo.AntecedentesEA, Acativo = modelo.Acativo};

            if (model.IdMotivoConsulta == 0)
            {
                db.Entry<MotivoConsulta>(model).State = EntityState.Added;
            }
            else
            {
                db.Entry<MotivoConsulta>(model).State = EntityState.Modified;
            }

            db.SaveChanges();
            return true;
        }

       
    }
}
