using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Threading;
using Dermasoft.core.data;
using System;


using System.Linq;
using System.Web;

namespace Dermasoft.core.business
{
   public class ArchivoImagenFactory
    {
        public DermasoftEntities db = new DermasoftEntities();
        public int IdArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public Nullable<int> IdVisita { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }

        public virtual Visita Visitas { get; set; }


        public HttpPostedFileBase Archivo { get; set; }
        public string Error { get; set; }



       public bool GuardarImagen()
       {
           var model = new ArchivoImagen {IdArchivo = IdArchivo, NombreArchivo = NombreArchivo, IdVisita = IdVisita, Extension = Extension, ContentType = ContentType};

           if (model.IdVisita == 0)
           {
               db.Entry<ArchivoImagen>(model).State = EntityState.Added;
           }
           else
           {
               db.Entry<ArchivoImagen>(model).State = EntityState.Modified;
           }

           db.SaveChanges();
           return true;
       }

       public bool Guardar()
       {
           if (Archivo != null && Archivo.ContentLength > 0 && IdVisita != 0)
           {

               var directorio = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DirectorioUpload"] + "/Imagenes");
               var dir = new FileInfo(directorio);
               //if (!dir.Exists)
               //{
               //    dir.Directory.Create();
               //}
               var cadena = Archivo.FileName.Split('.');
               var archivo = new ArchivoImagen { NombreArchivo = Guid.NewGuid().ToString(), IdVisita = IdVisita ?? 0, Extension = cadena.Last().ToLower() };

               // Check extension
               if (archivo.Extension == "gif")
               {
                   archivo.ContentType = "image/gif";
               }
               else if (archivo.Extension == "jpg" || archivo.Extension == "jpeg")
               {
                   archivo.ContentType = "image/jpeg";
               }
               else if (archivo.Extension == "png")
               {
                   archivo.ContentType = "image/png";
               }

               try
               {
                
                   Archivo.SaveAs(directorio + "/" + archivo.NombreArchivo + "." + archivo.Extension);
                   db.ArchivoImagen.Add(archivo);
                   db.SaveChanges();
                   NombreArchivo = archivo.NombreArchivo;
                   return true;
               }
               catch (Exception)
               {
                   Error = "Error al Guardar en DB";
                   return false;
               }
           }

           Error = "Registro No definido";
           return false;
       }

    }
}
