using System;
using System.Collections.Generic;
using System.Data.Entity;
using Dermasoft.core.data;
using System.Linq;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace Dermasoft.web.Models
{
    public class NotaModel
    {
        private static readonly DermasoftEntities Db = new DermasoftEntities();

        public int IdVisita { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public static void Save(string notasNuevas, int idConsulta, int idVisita, object tipoNota)
        {
            if (!notasNuevas.IsNullOrWhiteSpace())
            {
                var idTipoNota = (int)tipoNota;
                var notas = notasNuevas.Split(',');
                var result = new List<Nota>();
                notas.ForEach(nota => 
                    result.Add( new Nota 
                    {
                        IdVisita = idVisita, 
                        IdTipoNota = idTipoNota, 
                        Descripcion = nota
                    }));
                Db.Nota.AddRange(result);
                Db.SaveChanges();
            }
        }

        public static List<NotaModel> GetAll(int idConsulta, object tipoNota)
        {
            var idTipoNota = (int) tipoNota;
            var result = new List<NotaModel>();
            var visitas = Db.Visita.Where(x => x.IdMotivoConsulta == idConsulta).ToList();
            if (visitas.Any())
            {
                visitas.ForEach(visita =>
                    visita.Notas1.Where(x => x.IdTipoNota == idTipoNota)
                    .ForEach(nota => result.Add(new NotaModel
                    {
                        IdVisita = visita.IdVisita,
                        Descripcion = nota.Descripcion,
                        Fecha = visita.FechaVisita
                    })));
            }
            return result;
        }

        public static List<NotaModel> GetAllHistory(int idConsulta, int idVisita, object tipoNota)
        {
            var idTipoNota = (int)tipoNota;
            var result = new List<NotaModel>();
            var visitas = Db.Visita.Where(x => x.IdMotivoConsulta == idConsulta && x.IdVisita == idVisita).ToList();
            if (visitas.Any())
            {
                visitas.ForEach(visita =>
                    visita.Notas1.Where(x => x.IdTipoNota == idTipoNota)
                    .ForEach(nota => result.Add(new NotaModel
                    {
                        IdVisita = visita.IdVisita,
                        Descripcion = nota.Descripcion,
                        Fecha = visita.FechaVisita
                    })));
            }
            return result;
        }

    }
}