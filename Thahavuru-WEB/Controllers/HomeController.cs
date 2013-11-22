using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thahavuru_WEB.Core;
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
            SessionDataManager SM = new SessionDataManager();

            if (SM.IsExists(SessionDataManager.Key.matchingResults) && SM.Get(SessionDataManager.Key.matchingResults) != null)
            {
                List<ImageModel> ResultImages = (List<ImageModel>)SM.Get(SessionDataManager.Key.matchingResults); 
                return PartialView(ResultImages);
            }
            return PartialView(null);
        }


        public JsonResult uploadFiles(string ImageName)
        {
            
            return Json(true, JsonRequestBehavior.AllowGet);// indexer();
        }

        //[AsyncTimeout(300000)]
        public ActionResult Upload(HttpPostedFileBase FileData, FormCollection forms)
        {
            var file = Request.Files["Filedata"];
            string savePath = Server.MapPath(@"~\Content\" + file.FileName);
            file.SaveAs(savePath);


            List<ImageModel> ResultImages = new List<ImageModel>();

            ImageModel img = new ImageModel();//DSC05296.JPG
            img.MatchedImageURI = @"http://localhost/imgs/user%20(1).jpg";
            ImageModel img2 = new ImageModel();//DSC05296.JPG
            img2.MatchedImageURI = @"http://localhost/imgs/user%20(2).jpg";
            ImageModel img3 = new ImageModel();//DSC05296.JPG
            img3.MatchedImageURI = @"http://localhost/imgs/user%20(3).jpg";
            ImageModel img4 = new ImageModel();//DSC05296.JPG
            img4.MatchedImageURI = @"http://localhost/imgs/user%20(4).jpg";
            ImageModel img5 = new ImageModel();//DSC05296.JPG
            img5.MatchedImageURI = @"http://localhost/imgs/user%20(5).jpg";
            ImageModel img6 = new ImageModel();//DSC05296.JPG
            img6.MatchedImageURI = @"http://localhost/imgs/user%20(6).jpg";
            ImageModel img7 = new ImageModel();//DSC05296.JPG
            img7.MatchedImageURI = @"http://localhost/imgs/user%20(7).jpg";
            ImageModel img8 = new ImageModel();//DSC05296.JPG
            img8.MatchedImageURI = @"http://localhost/imgs/user%20(8).jpg";
            ImageModel img9 = new ImageModel();//DSC05296.JPG
            img9.MatchedImageURI = @"http://localhost/imgs/user%20(9).jpg";
            ImageModel img10 = new ImageModel();//DSC05296.JPG
            img10.MatchedImageURI = @"http://localhost/imgs/user%20(10).jpg";


            ResultImages.Add(img);
            ResultImages.Add(img2);
            ResultImages.Add(img3);
            ResultImages.Add(img4);
            ResultImages.Add(img5);
            ResultImages.Add(img6);
            ResultImages.Add(img7);
            ResultImages.Add(img8);
            ResultImages.Add(img9);
            ResultImages.Add(img10);
            

            SessionDataManager SM = new SessionDataManager();
            SM.Clear(SessionDataManager.Key.matchingResults);
            SM.Set(SessionDataManager.Key.matchingResults, ResultImages);

              


            return Content(Url.Content(@"~\Content\" + file.FileName));
        }

    }
}
