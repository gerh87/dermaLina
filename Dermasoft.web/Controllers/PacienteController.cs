using System.Data.Entity.Core.Objects;
using System.Web.Mvc;
using System.Web.WebPages;
using Dermasoft.web.Models;
using System.Data.Entity;

namespace Dermasoft.web.Controllers
{
    [Authorize]
    public class PacienteController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index", PacienteModel.Init());
        }
        [HttpGet]
        public ActionResult PacienteFind(string query)
        {
            var model = PacienteModel.Find(query);
            if (model.IdPaciente <= 0 || query.IsEmpty())
            {
                return PartialView("_PacienteLista", PacienteModel.GetAll());
            }
            return PartialView("_PacienteInfo", model);
        }
        [HttpGet]
        public ActionResult PacienteGet(int idPaciente)
        {
            var model = PacienteModel.Get(idPaciente, false);
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PacienteInfo", model);
        }
        public ActionResult PacienteCreate(int id)
        {
            ViewBag.Action = id == 0 ? "Nuevo" : "Editar";
            return View("_PacienteForm", PacienteModel.Get(id));
        }
        [HttpPost]
        public ActionResult PacienteSave(PacienteModel model)
        {
            model.FillCombos();
            if (!ModelState.IsValid)
            {
                ViewBag.Action = model.IdPaciente == 0 ? "Nuevo" : "Editar";
                
                return View("_PacienteForm", model);
            }
            return PacienteModel.Save(model) ? View("Index", PacienteModel.Init()) : View("_PacienteForm", model);
        }
        [HttpPost]
        [Authorize(Users = "admin")]
        public ActionResult PacienteDelete(int idPaciente)
        {
            PacienteModel.Delete(idPaciente);
            return PartialView("_PacienteLista", PacienteModel.GetAll());
        }
        [HttpGet]
        public ActionResult PacienteAutoComplete(string query)
        {
            var resultados = PacienteModel.FindJson(query);
            return Json(resultados, JsonRequestBehavior.AllowGet);
        }

    }
}
