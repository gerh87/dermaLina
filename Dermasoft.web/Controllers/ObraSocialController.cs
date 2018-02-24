
using System.Web.Mvc;
using Dermasoft.web.Models;

namespace Dermasoft.web.Controllers
{
    [Authorize]
    public class ObraSocialController : Controller
    {
        public ActionResult Index()
        {
            return View(ObraSocialModel.GetAll());
        }

        public ActionResult ObraSocialCreate(int idObraSocial)
        {
            ViewBag.Action = idObraSocial == 0 ? "Nueva" : "Editar";
            return View("_ObraSociaForm", ObraSocialModel.Get(idObraSocial));
        }

        public ActionResult ObraSocialSave(ObraSocialModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = model.IdObraSocial == 0 ? "Nueva" : "Editar";
                return View("_ObraSociaForm", model);
            }
            return ObraSocialModel.Save(model) ? View("Index", ObraSocialModel.GetAll()) : View("_ObraSociaForm", model);
        }

        public ActionResult ObraSocialDelete(int idObraSocial)
        {
            if (!ObraSocialModel.Delete(idObraSocial))
            {
                ModelState.AddModelError("", "EXISTEN PACIENTES CON LA OBRA SOCIAL QUE DESEA ELIMINAR");
            }
            
            return PartialView("Index", ObraSocialModel.GetAll());
        }
    }
}