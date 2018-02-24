using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using Dermasoft.web.Models;
using Microsoft.Ajax.Utilities;

namespace Dermasoft.web.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(string Usuario, string Contrasenia)
        {
            var exist = UsuarioModel.Exist(Usuario, Contrasenia);
            if (exist != null)
            {
                FormsAuthentication.SetAuthCookie(Usuario, false);
                return RedirectToAction("Index", "Paciente");
            }
            return View();
        }

        [HttpGet]
        public ActionResult UsuarioLista()
        {
            return View("UsuarioLista", UsuarioModel.GetAll());
        }
        [HttpGet]
        public ActionResult UsuarioGet(int idUsuario)
        {
            ViewBag.Action = idUsuario == 0 ? "Nuevo" : "Editar";
            return View("_UsuarioForm", UsuarioModel.Get(idUsuario));
        }
        [HttpPost]
        public ActionResult UsuarioSave(UsuarioModel model)
        {
            if (ModelState.IsValid)
            {
                if (UsuarioModel.Save(model))
                {
                    return View("UsuarioLista", UsuarioModel.GetAll());
                }
            }
            model.FillCombos();
            ViewBag.Action = model.IdUsuario == 0 ? "Nuevo" : "Editar";
            return View("_UsuarioForm", model);
        }
        public ActionResult UsuarioDelete(int idUsuario)
        {
            if (idUsuario == MvcApplication.IdUser)
            {
                ModelState.AddModelError("", "NO PUEDE ELIMINAR EL USUARIO CON EL QUE ESTA LOGEADO");
                return PartialView("UsuarioLista", UsuarioModel.GetAll());
            }
            UsuarioModel.Delete(idUsuario);
            return PartialView("UsuarioLista", UsuarioModel.GetAll());
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Usuario");
        }


      
    }
}
