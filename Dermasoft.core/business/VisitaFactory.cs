using System.Web;
using Dermasoft.core.data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dermasoft.core.business
{

    public class VisitaItemModel
    {
        public int IdVisita { get; set; }
        public DateTime? FechaVisita { get; set; }
        public string Motivo { get; set; }
        public bool Activo { get; set; }

    }
    public class VisitaFactory
    {

        public DermasoftEntities db = new DermasoftEntities();


        public int IdVisita { get; set; }
        public Nullable<System.DateTime> FechaVisita { get; set; }

        public List<VisitaItemModel> ListaVisitas { get; set; }

        public int IdPaciente { get; set; }

        public string Error { get; set; }

        public HttpPostedFileBase Archivo { get; set; }

        public RelVisitaMotivo Consulta { get; set; }
        public bool ObtenerVisitas()
        {
            ListaVisitas = db.Visita.Where(s => s.IdPaciente == IdPaciente)
                .Select(v => new VisitaItemModel
                {
                    IdVisita = (int) v.IdVisita,
                    FechaVisita = v.FechaVisita,
                    Motivo = v.RelVisitaMotivo.FirstOrDefault(s => s.Visita.IdVisita == v.IdVisita).MotivoConsultas.MotivoConsulta,
                    Activo = v.RelVisitaMotivo.FirstOrDefault(s => s.Visita.IdVisita == v.IdVisita).MotivoConsultas.Acativo??false
                }).ToList();
            return true;
        }


    

        public bool save(Visita modelo)
        {
            var model = new Visita {IdVisita = modelo.IdVisita, FechaVisita = DateTime.Now, IdPaciente = modelo.IdPaciente};

            if(model.IdVisita == 0)
            {
                db.Entry<Visita>(model).State = EntityState.Added;
            }
            else
            {
                db.Entry<Visita>(model).State = EntityState.Modified;
            }

            db.SaveChanges();
            return true;
        }


    }
}
