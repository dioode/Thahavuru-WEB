using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thahavuru_WEB.App_Start;
using Thahavuru_WEB.Models;
using System.IO;

namespace Test_Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Header()
        {
            return PartialView();
        }

        public ActionResult FaceRecognizerIndex()
        {
            return PartialView();
        }
        public ActionResult Footer()
        {
            return PartialView();
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

            
                // get the exact file name from the path
                String strFile = System.IO.Path.GetFileName(savePath);

                // create an instance fo the web service
                Thahavuru_WEB.ThahavuruServiceReference.ThahavuruFaceRecognitionServiceClient srv = new Thahavuru_WEB.ThahavuruServiceReference.ThahavuruFaceRecognitionServiceClient();
                    


                // get the file information form the selected file
                FileInfo fInfo = new FileInfo(savePath);

                // get the length of the file to see if it is possible
                // to upload it (with the standard 4 MB limit)
                long numBytes = fInfo.Length;
                double dLen = Convert.ToDouble(fInfo.Length / 1000000);

                // Default limit of 4 MB on web server
                // have to change the web.config to if
                // you want to allow larger uploads
                if (dLen < 4)
                {
                    //file.SaveAs(savePath);
                    // set up a file stream and binary reader for the
                    // selected file
                    FileStream fStream = new FileStream(savePath,
                    FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fStream);

                    // convert the file to a byte array
                    byte[] data = br.ReadBytes((int)numBytes);
                    br.Close();

                    // pass the byte array (file) and file name to the web service
                    string sTmp = srv.UploadFile(data, strFile);
                    fStream.Close();
                    fStream.Dispose();

                    // this will always say OK unless an error occurs,
                    // if an error occurs, the service returns the error  message
                    /// MessageBox.Show("File Upload Status: " + sTmp, "File Upload");
                }
                else
                {
                    // Display message if the file was too large to upload
                    TempData.Add("The file selected exceeds the size limit for uploads.",0);
                    ViewBag.message = "The file selected exceeds the size limit for uploads.";
                }
           


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
