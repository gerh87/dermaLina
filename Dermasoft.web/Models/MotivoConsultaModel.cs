using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Dermasoft.core.data;
using Microsoft.Ajax.Utilities;

namespace Dermasoft.web.Models
{
    public class MotivoConsultaModel
    {
        private static readonly DermasoftEntities Db = new DermasoftEntities();
        public int IdMotivoConsulta { get; set; }
        public string ID { get; set; }

        [Required]
        [DisplayName("Motivo de la consulta")]
        public string MotivoConsultaTitulo { get; set; }

        [Required]
        [DisplayName("Antecedentes EA")]
        public string AntecedentesEA { get; set; }
        public DateTime UltimaVisita { get; set; }
        public int NumeroVisitas { get; set; }
        public bool? Activo { get; set; }

        public int IdPaciente { get; set; }

        public PacienteModel Paciente { get; set; }

        public List<VisitaModel> Visitas { get; set; }

        public List<ArchivoImagenModel> Imagenes { get; set; }
        public List<NotaModel> Notas { get; set; }
        public string NotasNuevas { get; set; }

        public List<NotaModel> Examenes { get; set; }
        public string ExamenesNuevos { get; set; }

        public List<NotaModel> Conductas { get; set; }
        public string ConductasNuevas { get; set; }


        public List<AlergiaModel> Alergias { get; set; }

        public List<AntecedenteModel> Antecedentes { get; set; }


        public void FillCombos()
        {
            Notas = NotaModel.GetAll(this.IdMotivoConsulta, NotaEnum.Nota).OrderByDescending(x => x.Fecha).Take(5).ToList();
            Examenes = NotaModel.GetAll(this.IdMotivoConsulta, NotaEnum.ExamenFisico).OrderByDescending(x => x.Fecha).Take(5).ToList();
            Conductas = NotaModel.GetAll(this.IdMotivoConsulta, NotaEnum.Conducta).OrderByDescending(x => x.Fecha).Take(5).ToList();
            Imagenes = ArchivoImagenModel.GetAll(this.IdMotivoConsulta);
            Alergias = AlergiaModel.GetAll(this.IdPaciente);
            Antecedentes = AntecedenteModel.GetAll(this.IdPaciente);
        }

        public static List<MotivoConsultaModel> GetAll(int idPaciente)
        {
            var list = Db.MotivoConsulta.Where(x => x.IdPaciente == idPaciente).ToList();
            if (list.Count > 0)
            {
                return list
                    .Select(x => new MotivoConsultaModel
                    {
                        Activo = x.Activo,
                        AntecedentesEA = x.AntecedentesEA,
                        IdMotivoConsulta = x.IdMotivoConsulta,
                        MotivoConsultaTitulo = x.MotivoConsulta1,
                        IdPaciente = x.IdPaciente,
                        NumeroVisitas = VisitaModel.Count(x.IdMotivoConsulta),
                        UltimaVisita = VisitaModel.GetFecha(x.IdMotivoConsulta),
                        Visitas = VisitaModel.GetAllEntity(x.Visitas),
                        Imagenes = ArchivoImagenModel.GetAll(x.IdMotivoConsulta)
                    }).ToList();
            }
            return new List<MotivoConsultaModel>();
        }
        public static List<MotivoConsultaModel> GetAllEntity(ICollection<MotivoConsulta> consultas)
        {
            if (consultas.Count > 0)
            {

                return consultas.Select(x => new MotivoConsultaModel
                {
                    IdMotivoConsulta = x.IdMotivoConsulta,
                    Activo = x.Activo,
                    AntecedentesEA = x.AntecedentesEA,
                    MotivoConsultaTitulo = x.MotivoConsulta1,
                    IdPaciente = x.IdPaciente,
                    UltimaVisita = VisitaModel.GetFecha(x.IdMotivoConsulta),
                    Visitas = VisitaModel.GetAllEntity(x.Visitas),
                    Imagenes = ArchivoImagenModel.GetAll(x.IdMotivoConsulta),
                }).ToList();
            }
            return new List<MotivoConsultaModel>();
        }
        public static MotivoConsultaModel Get(int id, int idPaciente)
        {
            var entity = Db.MotivoConsulta.FirstOrDefault(x => x.IdMotivoConsulta == id);
            if (entity != null)
            {
                return new MotivoConsultaModel
                {
                    Activo = entity.Activo,
                    AntecedentesEA = entity.AntecedentesEA,
                    IdMotivoConsulta = entity.IdMotivoConsulta,
                    MotivoConsultaTitulo = entity.MotivoConsulta1,
                    IdPaciente = idPaciente,
                    ID = Guid.NewGuid().ToString().Substring(0, 10),
                    Visitas = VisitaModel.GetAll(entity.IdMotivoConsulta),
                    Imagenes = ArchivoImagenModel.GetAll(id),
                    Notas = NotaModel.GetAll(id, NotaEnum.Nota).OrderByDescending(x => x.Fecha).Take(5).ToList(),
                    Examenes = NotaModel.GetAll(id, NotaEnum.ExamenFisico).OrderByDescending(x => x.Fecha).Take(5).ToList(),
                    Conductas = NotaModel.GetAll(id, NotaEnum.Conducta).OrderByDescending(x => x.Fecha).Take(5).ToList(),
                    Alergias = AlergiaModel.GetAll(idPaciente),
                    Antecedentes = AntecedenteModel.GetAll(idPaciente)
                };
            }
            return new MotivoConsultaModel
            {
                IdMotivoConsulta = 0,
                IdPaciente = idPaciente,
                ID = Guid.NewGuid().ToString().Substring(0, 10),
                Visitas = new List<VisitaModel>(),
                Imagenes = new List<ArchivoImagenModel>(),
                Notas = new List<NotaModel>(),
                Examenes = new List<NotaModel>(),
                Conductas = new List<NotaModel>(),
                Alergias = AlergiaModel.GetAll(idPaciente),
                Antecedentes = AntecedenteModel.GetAll(idPaciente)
            };
        }
        public static MotivoConsultaModel GetHistory(int id, int idPaciente)
        {
            var entity = Db.MotivoConsulta.FirstOrDefault(x => x.IdMotivoConsulta == id);
            if (entity != null)
            {
                return new MotivoConsultaModel
                {
                    IdMotivoConsulta = entity.IdMotivoConsulta,
                    IdPaciente = idPaciente,
                    MotivoConsultaTitulo = entity.MotivoConsulta1,
                    Visitas = VisitaModel.GetAllHistory(entity.IdMotivoConsulta)
                };
            }
            return null;
        }
        public static bool Save(MotivoConsultaModel model)
        {
            try
            {
                var entity = Db.MotivoConsulta.FirstOrDefault(x => x.IdMotivoConsulta == model.IdMotivoConsulta) ?? new MotivoConsulta { IdMotivoConsulta = 0 };
                entity.Activo = model.Activo ?? false;
                entity.MotivoConsulta1 = model.MotivoConsultaTitulo;
                entity.AntecedentesEA = model.AntecedentesEA;
                entity.IdPaciente = model.IdPaciente;

                Db.Entry(entity).State = entity.IdMotivoConsulta == 0 ? EntityState.Added : EntityState.Modified;
                Db.SaveChanges();

                var visita = VisitaModel.Save(entity.IdMotivoConsulta);
                NotaModel.Save(model.NotasNuevas, entity.IdMotivoConsulta, visita.IdVisita, NotaEnum.Nota);
                NotaModel.Save(model.ExamenesNuevos, entity.IdMotivoConsulta, visita.IdVisita, NotaEnum.ExamenFisico);
                NotaModel.Save(model.ConductasNuevas, entity.IdMotivoConsulta, visita.IdVisita, NotaEnum.Conducta);
                ArchivoImagenModel.Update(visita.IdVisita, model.ID);
                    
                Db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        
    }
}