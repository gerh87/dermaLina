using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Dermasoft.core.data;
using WebGrease.Css.Extensions;

namespace Dermasoft.web.Models
{
    public class ArchivoImagenModel
    {
        public int IdArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public int? IdVisita { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public DateTime Fecha { get; set; }

        private static readonly DermasoftEntities Db = new DermasoftEntities();

        public static List<ArchivoImagenModel> GetAll(int idConsulta)
        {
            var result = new List<ArchivoImagenModel>();
            var visitas = Db.Visita.Where(x => x.IdMotivoConsulta == idConsulta).ToList();
            if (visitas.Any())
            {
                visitas.ForEach(visita =>
                       visita.ArchivoImagen.ForEach(img => result.Add(new ArchivoImagenModel
                       {
                           ContentType = img.ContentType,
                           Extension = img.Extension,
                           IdArchivo = img.IdArchivo,
                           IdVisita = img.IdVisita,
                           NombreArchivo = img.NombreArchivo,
                           Fecha = visita.FechaVisita
                       })));
            }
            return result;
        }
        public static List<ArchivoImagenModel> GetAllHistory(int idConsulta, int idVisita)
        {
            var result = new List<ArchivoImagenModel>();
            var visitas = Db.Visita.Where(x => x.IdMotivoConsulta == idConsulta && x.IdVisita == idVisita).ToList();
            if (visitas.Any())
            {
                visitas.ForEach(visita =>
                       visita.ArchivoImagen.ForEach(img => result.Add(new ArchivoImagenModel
                       {
                           ContentType = img.ContentType,
                           Extension = img.Extension,
                           IdArchivo = img.IdArchivo,
                           IdVisita = img.IdVisita,
                           NombreArchivo = img.NombreArchivo,
                           Fecha = visita.FechaVisita
                       })));
            }
            return result;
        }
        public static ArchivoImagenModel Get(int idArchivoImagen)
        {
            var entity = Db.ArchivoImagen.FirstOrDefault(x => x.IdArchivo == idArchivoImagen);
            if (entity != null)
            {
                return new ArchivoImagenModel
                {
                    ContentType = entity.ContentType,
                    Extension = entity.Extension,
                    IdArchivo = entity.IdArchivo,
                    IdVisita = entity.IdVisita,
                    NombreArchivo = entity.NombreArchivo
                };
            }
            return new ArchivoImagenModel {IdArchivo = 0};
        }

        public static List<ArchivoImagenModel> GetAllEntity(ICollection<ArchivoImagen> imagenes)
        {
            return imagenes
                .Select(x => new ArchivoImagenModel
                {
                    ContentType = x.ContentType,
                    Extension = x.Extension,
                    IdArchivo = x.IdArchivo,
                    IdVisita = x.IdVisita,
                    NombreArchivo = x.NombreArchivo
                }).ToList();
        }

        public static void Save(ArchivoImagenModel model)
        {
            var entity = new ArchivoImagen();
            entity.ContentType = model.ContentType;
            entity.Extension = model.Extension;
            entity.NombreArchivo = model.NombreArchivo;

            try
            {
                Db.Entry(entity).State = EntityState.Added;
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void Update(int idVisita, string ID)
        {
            var images = Db.ArchivoImagen.Where(x => x.ContentType.Equals(ID)).ToList();
            foreach (var item in images)
            {
                item.IdVisita = idVisita;
                item.ContentType = "image";
                Db.Entry(item).State = EntityState.Modified;
                Db.SaveChanges();
            }
        }

    }
}