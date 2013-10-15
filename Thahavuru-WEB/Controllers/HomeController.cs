using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Thahavuru_WEB.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ResultImages() 
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AjaxUpload(HttpPostedFileBase file)
        {
            // TODO: Add your business logic here and/or save the file

            // Return JSON
            //return new FileUploadJsonResult { Data = new { message = string.Format("{0} uploaded successfully.", System.IO.Path.GetFileName(file.FileName)) } };
            try
            {
                //HttpPostedFileBase file = Request.Files[0];
                byte[] imageSize = new byte[file.ContentLength];
                file.InputStream.Read(imageSize, 0, (int)file.ContentLength);
                //Image image = new Image()
                //{
                //    Name = file.FileName.Split('\\').Last(),
                //    Size = file.ContentLength,
                //    Title = fileTitle,
                //    ID = 1,
                //    Image1 = imageSize
                //};
                //db.Images.AddObject(image);
                //db.SaveChanges();
                //return RedirectToAction("Detail");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("uploadError", e);
            }

            return RedirectToAction("Index");// indexer();
        }

    }
}
