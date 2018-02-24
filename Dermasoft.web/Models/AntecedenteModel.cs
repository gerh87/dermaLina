using System;
using System.Collections.Generic;
using System.Linq;
using Dermasoft.core.data;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace Dermasoft.web.Models
{
    public class AntecedenteModel
    {

        private static readonly DermasoftEntities Db = new DermasoftEntities();
        public int IdAntecedente { get; set; }
        public int IdPaciente { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public static List<AntecedenteModel> GetAll(int idPaciente)
        {
            var result = new List<AntecedenteModel>();
            var antecedentes = Db.Antecedente.Where(x => x.IdPaciente == idPaciente).ToList();
            if (antecedentes.Any())
            {
                antecedentes.ForEach(antecedente => result.Add(new AntecedenteModel
                {
                    IdAntecedente = antecedente.IdAntecedente,
                    IdPaciente = antecedente.IdPaciente,
                    Descripcion = antecedente.Descipcion,
                    Fecha = antecedente.Fecha
                }));

            }
            return result.OrderByDescending(x => x.Fecha).ToList();
        }
        public static void Save(string antecedentesNuevos, int idPaciente)
        {
            if (!antecedentesNuevos.IsNullOrWhiteSpace())
            {
                var antecedentes = antecedentesNuevos.Split(',');
                var entities = new List<Antecedente>();
                if (antecedentes.Any())
                {
                    antecedentes.ForEach(antecedente => entities.Add(new Antecedente
                    {
                        IdPaciente = idPaciente,
                        Descipcion = antecedente,
                        Fecha = DateTime.Now
                    }));
                }
                Db.Antecedente.AddRange(entities);
                Db.SaveChanges();
            }
        }

    }
}