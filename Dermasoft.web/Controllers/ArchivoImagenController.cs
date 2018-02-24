using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Dermasoft.web.Controllers
{
    [Authorize]
    public class ArchivoImagenController : Controller
    {
        // GET: ArchivoImagen
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadFile(HttpPostedFileBase file, int? id)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/UploadFile"), fileName);
                file.SaveAs(path);
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("UploadFile");   
            
          
        }

        
    }
}