using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Dermasoft.core.data;

namespace Dermasoft.web.Models
{

    public class ObraSocialModel
    {
        private static readonly DermasoftEntities Db = new DermasoftEntities();

        public int IdObraSocial { get; set; }

        [Required]
        [DisplayName("Nombre de Obra Social")]
        public string NombreObraSocial { get; set; }
        [Required]
        public string Clave { get; set; }
        public bool Activo { get; set; }


        public static ObraSocialModel Get(int id)
        {
            var entity = Db.ObraSocial.FirstOrDefault(x => x.IdObraSocial == id);
            if ( entity != null)
            {
                return new ObraSocialModel
                {
                    IdObraSocial = entity.IdObraSocial,
                    Activo = entity.Activo,
                    Clave = entity.Clave,
                    NombreObraSocial = entity.NombreObraSocial
                };
            }
            return new ObraSocialModel();
        }
        public static ObraSocialModel GetEntity(ObraSocial obraSocial)
        {
            if (obraSocial != null)
            {
                return new ObraSocialModel
                {
                    IdObraSocial = obraSocial.IdObraSocial,
                    Activo = obraSocial.Activo,
                    Clave = obraSocial.Clave,
                    NombreObraSocial = obraSocial.NombreObraSocial
                };
            }
            return new ObraSocialModel();
        }

        public static List<ObraSocialModel> GetAll()
        {
            return Db.ObraSocial.OrderBy(x => x.IdObraSocial).ToList()
                .Select(x => new ObraSocialModel
                {
                    IdObraSocial = x.IdObraSocial,
                    Activo = x.Activo,
                    Clave = x.Clave,
                    NombreObraSocial = x.NombreObraSocial
                }).ToList();
        }

        public static bool Save(ObraSocialModel model)
        {
            try
            {
                var entity = Db.ObraSocial.FirstOrDefault(x => x.IdObraSocial == model.IdObraSocial) ?? new ObraSocial { IdObraSocial = 0 };
                entity.NombreObraSocial = model.NombreObraSocial;
                entity.Clave = model.Clave;
                entity.Activo = true;

                Db.Entry(entity).State = entity.IdObraSocial == 0 ? EntityState.Added : EntityState.Modified;
                Db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool Delete(int idObraSocial)
        {
            var pacientes = Db.Paciente.Where(x => x.IdObraSocial == idObraSocial).ToList();
            var model = Db.ObraSocial.Where(x => x.IdObraSocial== idObraSocial).ToList();
            if (pacientes.Count > 0) return false;
            Db.ObraSocial.Remove(model.FirstOrDefault(x => x.IdObraSocial == idObraSocial));
            Db.SaveChanges();
            return true;

        }

        public static int Count()
        {
            return Db.ObraSocial.Count();
        }
    }
}