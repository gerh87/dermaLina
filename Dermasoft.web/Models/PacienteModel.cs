using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

using System.Web.Mvc;
using System.Web.UI.WebControls;
using Dermasoft.core.data;


namespace Dermasoft.web.Models
{
    public class PacienteModel
    {
        private static readonly DermasoftEntities Db = new DermasoftEntities();

        public int IdPaciente { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        [DisplayName("Fecha de Nacimiento")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [DisplayName("DNI")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} debe contener solo numeros.")]
        public string Dni { get; set; }

        [Required]
        public int Edad { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Telefono { get; set; }

        public string AlergiasNuevas { get; set; }
        public string AntecedentesNuevos { get; set; }

        [Required]
        [DisplayName("Obra Social")]
        public int IdObraSocial { get; set; }

        public int PacienteCount { get; set; }
        public int ObraSocialCount { get; set; }
        public List<PacienteModel> Pacientes { get; set; }

        public string Busqueda { get; set; }
        public bool ContainsConsultas { get; set; }
        public ObraSocialModel ObraSocial { get; set; }
        public List<MotivoConsultaModel> Consultas { get; set; }
        public List<SelectListItem> ObrasSociales { get; set; }
        public virtual List<AlergiaModel> Alergias { get; set; }
        public virtual List<AntecedenteModel> Antecedentes { get; set; }
        [DisplayName("Número de Afiliado")]
        public string NroAfiliado { get; set; }

        public static PacienteModel Init()
        {
            return new PacienteModel
            {
                PacienteCount = Count(),
                ObraSocialCount = ObraSocialModel.Count(),
                Pacientes = GetAll()
            };
        }

        private static int Count()
        {
            return Db.Paciente.Count();
        }

        public static PacienteModel Get(int id, bool isCreate = true)
        {
            Db.Database.Initialize(true);
            var entity = Db.Paciente.FirstOrDefault(x => x.IdPaciente == id);
            if (entity != null)
            {
                var result = new PacienteModel
                {
                    Nombre = entity.Nombre,
                    IdPaciente = entity.IdPaciente,
                    Apellido = entity.Apellido,
                    Dni = entity.Dni,
                    Edad = entity.Edad,
                    FechaNacimiento = entity.FechaNacimiento,
                    Telefono = entity.Telefono,
                    Direccion = entity.Direccion,
                    IdObraSocial = entity.IdObraSocial,
                    ObraSocial = ObraSocialModel.Get(entity.IdObraSocial),
                    Consultas = MotivoConsultaModel.GetAll(entity.IdPaciente), 
                    Antecedentes = isCreate? AntecedenteModel.GetAll(entity.IdPaciente) : null,
                    Alergias = isCreate? AlergiaModel.GetAll(entity.IdPaciente) : null,
                    ObrasSociales = GetObrasSociales(),
                    NroAfiliado = entity.NroAfiliado
                 
                };
                return result;
            }
            return new PacienteModel
            {
                IdPaciente = 0,
                FechaNacimiento = DateTime.Now,
                Antecedentes = new List<AntecedenteModel>(),
                Alergias = new List<AlergiaModel>(),
                ObrasSociales = GetObrasSociales()
            };
        }
        public static PacienteModel Find(string query)
        {
            if (String.IsNullOrEmpty(query)) return new PacienteModel {IdPaciente = 0};

            if (query.Contains("-"))
                query = query.Split('-')[0].Trim();

            var entity = Db.Paciente.FirstOrDefault(r => r.Dni == query);
            return entity != null ? Get(entity.IdPaciente) : new PacienteModel { IdPaciente = 0};
        }
        public static bool Save(PacienteModel model)
        {
            try
            {
                var entity = Db.Paciente.FirstOrDefault(x => x.IdPaciente == model.IdPaciente) ?? new Paciente ();
                entity.Nombre = model.Nombre;
                entity.Apellido = model.Apellido;
                entity.Dni = model.Dni;
                entity.Edad = model.Edad;
                entity.FechaNacimiento = model.FechaNacimiento;
                entity.Direccion = model.Direccion;
                entity.Telefono = model.Telefono;
                entity.IdObraSocial = model.IdObraSocial;
                entity.NroAfiliado = model.NroAfiliado;

                Db.Entry(entity).State = entity.IdPaciente == 0 ? EntityState.Added : EntityState.Modified;
                Db.SaveChanges();

                AlergiaModel.Save(model.AlergiasNuevas, entity.IdPaciente);
                AntecedenteModel.Save(model.AntecedentesNuevos, entity.IdPaciente);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static List<PacienteModel> GetAll()
        {
            return Db.Paciente.OrderByDescending(x => x.IdPaciente).Take(6)
                .Select(s => new PacienteModel
                {
                    IdPaciente = s.IdPaciente,
                    Nombre = s.Nombre,
                    Apellido = s.Apellido,
                    FechaNacimiento = s.FechaNacimiento,
                    Dni = s.Dni,
                    Edad = s.Edad,
                    Telefono = s.Telefono,
                    ContainsConsultas = s.MotivoConsultas.Count > 0
                }).ToList();

        }
        public static List<SelectListItem> GetObrasSociales()
        {
            return ObraSocialModel.GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.IdObraSocial.ToString(),
                    Text = s.NombreObraSocial
                }).ToList();
        }
        public static void Delete(int id)
        {
            var model = Db.Paciente.FirstOrDefault(x => x.IdPaciente == id);
            if (model == null) return;
            Db.Alergia.RemoveRange(Db.Alergia.Where(x => x.IdPaciente == model.IdPaciente));
            Db.Antecedente.RemoveRange(Db.Antecedente.Where(x => x.IdPaciente == model.IdPaciente));
            model.Alergias.Clear();
            model.Antecedentes.Clear();
            Db.Paciente.Remove(model);
            Db.SaveChanges();
        }
        public static List<QueryResultModel> FindJson(string query)
        {
            return Db.Paciente
                    .Where(s => (s.Dni.Contains(query) || (s.Nombre + " " + s.Apellido).Contains(query))).OrderBy(s => s.Dni)
                    .Take(10)
                    .Select(s => new QueryResultModel
                            {
                                value = s.Dni + "-" + s.Nombre + " " + s.Apellido
                            }).ToList();
        }

        public void FillCombos()
        {
            Antecedentes = AntecedenteModel.GetAll(this.IdPaciente);
            Alergias = AlergiaModel.GetAll(this.IdPaciente);
            ObrasSociales = GetObrasSociales();
        }

    }
}