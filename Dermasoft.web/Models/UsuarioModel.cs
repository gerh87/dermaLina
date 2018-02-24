using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Dermasoft.core.data;

namespace Dermasoft.web.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }

        [DisplayName("Nombre de Usuario")]
        [Required]
        public string NombreUsuario { get; set; }

        [DisplayName("Apellido de Usuario")]
        [Required]
        public string ApellidoUsuario { get; set; }
        
        [DisplayName("Nick de Usuario")]
        [Required]
        public string Usuario { get; set; }


        [DisplayName("Contraseña")]
        [StringLength(20, MinimumLength = 8)]
        [Required]
        public string ContraseniaNueva1 { get; set; }


        [DisplayName("Contraseña Verificacion")]
        [System.ComponentModel.DataAnnotations.Compare("ContraseniaNueva1", ErrorMessage = "Las Contraseñas deben coincidir.")]
        [Required]
        public string ContraseniaNueva2 { get; set; }

        public bool Activo { get; set; }
        public virtual RolModel Rol { get; set; }

        public List<SelectListItem> Roles { get; set; }

        private static readonly DermasoftEntities Db = new DermasoftEntities();

        public static string Exist(string nick, string pass)
        {
            var passEncrypt = encrypt(pass);
            var result = Db.Usuario.FirstOrDefault(u => u.Usuario1 == nick && u.Contrasenia == passEncrypt);
            
            if (result != null)
            {
                MvcApplication.IdUser = result.IdUsuario;
                MvcApplication.IsAdmin = result.IdRol == 1;
                
                return result.IdRol == 1 ? "admin" : "usuario";
            }
            return null;
        }

        public void FillCombos()
        {
            Roles = GetRoles();
        }

        public static UsuarioModel Get(int id)
        {
            var entity =  Db.Usuario.FirstOrDefault( x => x.IdUsuario == id);
            if (entity != null)
            {
                return new UsuarioModel
                {
                    Activo = entity.Activo,
                    ApellidoUsuario = entity.ApellidoUsuario,
                    ContraseniaNueva1 = string.Empty,
                    ContraseniaNueva2 = string.Empty,
                    Rol = new RolModel{ IdRol = entity.Role.IdRol, Nombre = entity.Role.Nombre},
                    IdUsuario = entity.IdUsuario,
                    NombreUsuario = entity.NombreUsuario,
                    Usuario = entity.Usuario1,
                    Roles = GetRoles()
                };
            }
            return new UsuarioModel
            {
                IdUsuario = 0,
                ContraseniaNueva1 = string.Empty,
                ContraseniaNueva2 = string.Empty,
                Roles = GetRoles()
            };
        }

        public static List<UsuarioModel> GetAll()
        {
            return Db.Usuario.Select( x => new UsuarioModel
                {
                    Activo = x.Activo,
                    ApellidoUsuario = x.ApellidoUsuario,
                    Rol = new RolModel{ IdRol = x.Role.IdRol, Nombre = x.Role.Nombre},
                    IdUsuario = x.IdUsuario,
                    NombreUsuario = x.NombreUsuario,
                    Usuario = x.Usuario1
                }).ToList();
        }

        public static bool Save(UsuarioModel model)
        {
            var entity = Db.Usuario.FirstOrDefault(x => x.IdUsuario == model.IdUsuario) ?? new Usuario { IdUsuario = 0};
            entity.NombreUsuario = model.NombreUsuario;
            entity.ApellidoUsuario = model.ApellidoUsuario;
            entity.Usuario1 = model.Usuario;
            entity.Contrasenia = encrypt(model.ContraseniaNueva1);
            entity.Activo = true;
            entity.IdRol = model.Rol.IdRol;
            Db.Entry(entity).State = entity.IdUsuario == 0 ? EntityState.Added : EntityState.Modified;
            Db.SaveChanges();
            return true;
        }

        private static List<SelectListItem> GetRoles()
        {
            return RolModel.GetAll().Select(x => new SelectListItem
            {
                Value = x.IdRol.ToString(),
                Text = x.Nombre
            }).ToList();
        }

        public static void Delete(int idUsuario)
        {
            var model = Db.Usuario.FirstOrDefault(x => x.IdUsuario== idUsuario);
            if (model != null)
            {
                Db.Usuario.Remove(model);
                Db.SaveChanges();
            }
        }

        private static string encrypt(string value)
        {
            HashAlgorithm algorithm = SHA1.Create();  
            var bits = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            var sb = new StringBuilder();
            foreach (var b in bits)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

    }
}
