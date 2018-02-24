using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dermasoft.core.data;
namespace Dermasoft.web.Models
{

    public class VisitaModel
    {
        private static readonly DermasoftEntities Db = new DermasoftEntities();
        public int IdVisita { get; set; }
        public DateTime FechaVisita { get; set; }
        public MotivoConsultaModel MotivoConsulta { get; set; }
        public List<ArchivoImagenModel> Imagenes { get; set; }
        public List<NotaModel> Notas { get; set; }
        public List<NotaModel> Examenes { get; set; }
        public List<NotaModel> Conductas { get; set; }

        public static List<VisitaModel> GetAll(int idMotivoConsulta)
        {
            var result = Db.Visita.Where(x => x.IdMotivoConsulta == idMotivoConsulta)
                .Select(x => new VisitaModel
                {
                    IdVisita = x.IdVisita,
                    FechaVisita = x.FechaVisita
                }).ToList();

            return result;

        }
        public static List<VisitaModel> GetAllHistory(int idMotivoConsulta)
        {
            var result = Db.Visita.Where(x => x.IdMotivoConsulta == idMotivoConsulta).ToList();
            var list = result.Select(x => new VisitaModel
                {
                    IdVisita = x.IdVisita,
                    FechaVisita = x.FechaVisita,
                    Imagenes = ArchivoImagenModel.GetAllHistory(idMotivoConsulta, x.IdVisita),
                    Notas = NotaModel.GetAllHistory(idMotivoConsulta, x.IdVisita, NotaEnum.Nota),
                    Examenes = NotaModel.GetAllHistory(idMotivoConsulta, x.IdVisita, NotaEnum.ExamenFisico),
                    Conductas = NotaModel.GetAllHistory(idMotivoConsulta, x.IdVisita, NotaEnum.Conducta)
                }).ToList();

            return list;

        }
        public static VisitaModel Get(int idVisita)
        {
            var entity = Db.Visita.FirstOrDefault(x => x.IdVisita == idVisita);
            if (entity != null)
            {
                return new VisitaModel
                {
                    IdVisita = entity.IdVisita,
                    FechaVisita = entity.FechaVisita,
                    Imagenes = ArchivoImagenModel.GetAll(entity.IdVisita)
                };
            }

            return new VisitaModel { IdVisita = 0};
        }
        public static Visita Save(int idMotivoConsulta)
        {
            var visita = new Visita
            {
                IdMotivoConsulta = idMotivoConsulta,
                FechaVisita = DateTime.Now
            };
            Db.Entry(visita).State = EntityState.Added;
            Db.SaveChanges();
            return visita;
        }

        public static DateTime GetFecha(int idMotivoConsulta)
        {
            var entity = Db.Visita.OrderBy(x => x.FechaVisita).FirstOrDefault(x => x.IdMotivoConsulta == idMotivoConsulta);
            return entity == null? DateTime.Now : entity.FechaVisita;
        }

        public static List<VisitaModel> GetAllEntity(ICollection<Visita> visitas)
        {
            var result = visitas
                .Select(x => new VisitaModel
                {
                    IdVisita = x.IdVisita,
                    FechaVisita = x.FechaVisita,
                    Imagenes = ArchivoImagenModel.GetAllEntity(x.ArchivoImagen)
                }).ToList();

            return result;
        }

        public static int Count(int idMotivoConsulta)
        {
            return Db.Visita.Count(x => x.IdMotivoConsulta == idMotivoConsulta);
        }

        
    }
}