using Dermasoft.core.data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dermasoft.core.business
{
    public  class PacienteFactory
    {

        public string sPaciente { get; set; }
        public string Error { get; set; }
        public Paciente paciente { get; set; }
        public ObraSocial obraSocial { get; set; }
        public DermasoftEntities db = new DermasoftEntities();
        public List<string> errores { get; set; }

        public VisitaFactory visita { get; set; }

        public List<Paciente> getList()
        {
            return db.Paciente.ToList();
        }

        public Paciente getPaciente(int id)
        {
            return db.Paciente.FirstOrDefault(x => x.IdPaciente == id);
        }

        public  bool save(Paciente model)
        {
            if (model.IdPaciente == 0)
                db.Entry<Paciente>(model).State = EntityState.Added;
            else
                db.Entry<Paciente>(model).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public bool delete(int id)
        {
            var model = db.Paciente.FirstOrDefault(x => x.IdPaciente == id);
            if (model != null)
            {
                db.Paciente.Remove(model);
                db.SaveChanges();
            }
            return true;
        }

        public bool BuscarPaciente()
        {

            if (!String.IsNullOrEmpty(sPaciente))
            {
                if (sPaciente.Contains("-"))
                {
                    sPaciente = sPaciente.Replace("-", "#");
                    sPaciente = sPaciente.Split('#')[0].Trim();
                }

                paciente = db.Paciente.FirstOrDefault(r => r.Dni == sPaciente);
                obraSocial = db.ObraSocial.FirstOrDefault(o => o.IdObraSocial == paciente.IdObraSocial);
                
                if (paciente != null)
                {
                    visita = new VisitaFactory{IdPaciente = paciente.IdPaciente};
                    visita.ObtenerVisitas();
                    return true;
                }
                Error = "Paciente No encontrado";
                return false;

            }
            Error = "Parametro no establecido";
            return false;
        }
    }
}
