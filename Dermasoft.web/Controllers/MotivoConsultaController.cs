using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Dermasoft.web.Models;

namespace Dermasoft.web.Controllers
{
    [Authorize]
    public class MotivoConsultaController : Controller
    {
        [HttpGet]
        public ActionResult MotivoConsultaCreate(int id, int idPaciente)
        {
            ViewBag.Action = id == 0 ? "Nueva" : "Modificar";
            var model = MotivoConsultaModel.Get(id, idPaciente);
            Session["ID"] = model.ID;
            return View("ConsultaForm", model);
        }

        public ActionResult MotivoConsultaSave(MotivoConsultaModel model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Session["ID"].ToString();
                if (MotivoConsultaModel.Save(model))
                {
                    return View("ConsultaMessage");
                }
            }
            model.FillCombos();
            return View("ConsultaForm", model);
        }

        public ActionResult MotivoConsultaSaveImage()
        {
            var ID = Session["ID"].ToString();
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                var extension = file.ContentType.Split('/')[1];
                var name = Guid.NewGuid().ToString().Substring(0, 10) + "." + extension;
                if (file != null && file.ContentLength > 0)
                {
                    var originalDirectory = new DirectoryInfo(string.Format("{0}\\Images", ConfigurationManager.AppSettings["DirectorioUpload"])); //Server.MapPath(@"\")
                    string pathString = Path.Combine(originalDirectory.ToString(), "imagepath");
                    if (!Directory.Exists(pathString))
                        Directory.CreateDirectory(pathString);
                    var rute = string.Format("{0}\\{1}", pathString, name);
                    file.SaveAs(rute);

                    var image = new ArchivoImagenModel
                    {
                        ContentType = ID,
                        Extension = extension,
                        NombreArchivo = name
                    };
                    ArchivoImagenModel.Save(image);
                }
            }
            return Json(new { Message = "success" });
        }

        public ActionResult MotivoConsultaInfo(int id, int idPaciente)
        {
            var model = MotivoConsultaModel.GetHistory(id, idPaciente);
            return View("ConsultaHistorial", model);
        }
    }
}