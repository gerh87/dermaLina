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
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].FileName != "")
                {
                    var file = Request.Files[upload];
                    var extention = file.ContentType.Split('/')[1];

                    string path = AppDomain.CurrentDomain.BaseDirectory + "UploadFile/Images/imagepath";
                    string fileName = Guid.NewGuid().ToString().Substring(0, 10) + "." + extention;
                    Request.Files[upload].SaveAs(Path.Combine(path, fileName));
                    var image = new ArchivoImagenModel
                    {
                        ContentType = ID,
                        Extension = extention,
                        NombreArchivo = fileName
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