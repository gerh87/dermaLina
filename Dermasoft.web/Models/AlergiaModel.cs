using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dermasoft.core.data;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace Dermasoft.web.Models
{
    public class AlergiaModel
    {
        private static readonly DermasoftEntities Db = new DermasoftEntities();
        public int IdAlergia { get; set; }
        public int IdPaciente { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public static List<AlergiaModel> GetAll(int idPaciente)
        {
            var result = new List<AlergiaModel>();
            var alergias = Db.Alergia.Where(x => x.IdPaciente == idPaciente).ToList();
            if (alergias.Any())
            {
                alergias.ForEach(alergia => result.Add(new AlergiaModel
                {
                    IdAlergia = alergia.IdAlergia,
                    IdPaciente = alergia.IdPaciente,
                    Descripcion = alergia.Descripcion,
                    Fecha = alergia.Fecha
                }));

            }
            return result.OrderByDescending(x => x.Fecha).ToList();
        }
        public static void Save(string alergiasNuevas, int idPaciente)
        {
            if (!alergiasNuevas.IsNullOrWhiteSpace())
            {
                var alergias = alergiasNuevas.Split(',');
                var entities = new List<Alergia>();
                if (alergias.Any())
                {
                    alergias.ForEach(alergia => entities.Add(new Alergia
                    {
                        IdPaciente = idPaciente,
                        Descripcion = alergia,
                        Fecha = DateTime.Now
                    }));
                }
                Db.Alergia.AddRange(entities);
                Db.SaveChanges();
            }
        }

       
    }
}