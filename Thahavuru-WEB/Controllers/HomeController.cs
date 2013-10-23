using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thahavuru_WEB.Models;

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

        public PartialViewResult ResultImages() 
        {
            List<ImageModel> ResultImages = new List<ImageModel>();

            ResultImages.Add(new ImageModel());
            ResultImages.Add(new ImageModel());
            ResultImages.Add(new ImageModel());
            ResultImages.Add(new ImageModel());

            return PartialView(ResultImages);
        }


        public JsonResult uploadFiles(string ImageName)
        {
            
            return Json(true, JsonRequestBehavior.AllowGet);// indexer();
        }

        public ActionResult Upload(HttpPostedFileBase FileData, FormCollection forms)
        {
            var file = Request.Files["Filedata"];
            string savePath = Server.MapPath(@"~\Content\" + file.FileName);
            file.SaveAs(savePath);

            return Content(Url.Content(@"~\Content\" + file.FileName));
        }

    }
}
