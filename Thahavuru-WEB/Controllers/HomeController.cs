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

            if (SM.IsExists(SessionDataManager.Key.pageImageDataModel) && SM.Get(SessionDataManager.Key.pageImageDataModel) != null)
            {
                PageImageDataModel Pagedata = (PageImageDataModel)SM.Get(SessionDataManager.Key.pageImageDataModel);
                return PartialView(Pagedata);
            }
            return PartialView(null);
        }

        public JsonResult ClearResults(object data) 
        {
            SessionDataManager SM = new SessionDataManager();
            SM.Clear(SessionDataManager.Key.pageImageDataModel);
            //return RedirectToAction("ResultImages");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNextBackResultSet(string data)
        {
            SessionDataManager SM = new SessionDataManager();
            UserInterfaceModel UIM = null;
            PageImageDataModel pageData = null;

            if (SM.IsExists(SessionDataManager.Key.userInterfaceModel) && SM.Get(SessionDataManager.Key.userInterfaceModel) != null)
            {
                UIM = (UserInterfaceModel)SM.Get(SessionDataManager.Key.userInterfaceModel);
            }
            if (SM.IsExists(SessionDataManager.Key.pageImageDataModel) && SM.Get(SessionDataManager.Key.pageImageDataModel) != null)
            {
                pageData = (PageImageDataModel)SM.Get(SessionDataManager.Key.pageImageDataModel);
            }

            if (UIM == null || pageData == null) return Json(false, JsonRequestBehavior.AllowGet);

            if (data == "Next") 
            {
                int nextPage = pageData.PageNumber + 1;
                if (nextPage <= UIM.MaxLeaves)
                {
                    //get next page from service
                    ConvertModel(SM, UIM, pageData, nextPage);
                }
            }
            if (data == "Back")
            {
                int previousPage = pageData.PageNumber - 1;
                if (previousPage > 0)
                {
                    //get previous page by service
                    ConvertModel(SM, UIM, pageData, previousPage);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private static void ConvertModel(SessionDataManager SM, UserInterfaceModel UIM, PageImageDataModel pageData, int page)
        {

            pageData.ImageList = new List<ImageModel>();
            if (UIM.SearchingPerson.MatchedFaceIdSet[page] != null)
            {
                for (int i = 0; i < UIM.SearchingPerson.MatchedFaceIdSet[page].Count; i++)
                {
                    ImageModel img = new ImageModel();
                    img.MatchedImageURI = @"http://localhost/imgs/User%20(" + UIM.SearchingPerson.MatchedFaceIdSet[page][i] + ").jpg";
                    img.UserName = "User " + UIM.SearchingPerson.MatchedFaceIdSet[page][i];
                    img.Address = "Address " + UIM.SearchingPerson.MatchedFaceIdSet[page][i];
                    pageData.ImageList.Add(img);
                }
            }

            pageData.PageNumber = page;
            pageData.Next = pageData.PageNumber == (UIM.MaxLeaves) ? false : true;
            pageData.Back = pageData.PageNumber == 1 ? false : true;
            //if(pageData.PageNumber != 1)pageData.Back = true; else
            //if ((nextPage + 1) == 10) pageData.Next = false;

            SM.Clear(SessionDataManager.Key.pageImageDataModel);
            SM.Clear(SessionDataManager.Key.userInterfaceModel);

            SM.Set(SessionDataManager.Key.pageImageDataModel, pageData);
            SM.Set(SessionDataManager.Key.userInterfaceModel, UIM);
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
           

            UserInterfaceModel UIM = new UserInterfaceModel();
            UIM.PageNumber = 1;
            UIM.Next = true;
            UIM.Back = false;
            UIM.MaxLeaves = 10;

            UIM.SearchingPerson = new PersonVM();
            UIM.SearchingPerson.MatchedFaceIdSet.Add(1, new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(2, new List<int>() { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(3, new List<int>() { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(4, new List<int>() { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(5, new List<int>() { 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(6, new List<int>() { 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(7, new List<int>() { 61, 62, 63, 64, 65, 66, 67, 68, 69, 70 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(8, new List<int>() { 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(9, new List<int>() { 81, 82, 83, 84, 85, 86, 87, 88, 89, 90 });
            UIM.SearchingPerson.MatchedFaceIdSet.Add(10, new List<int>() { 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 });


            PageImageDataModel pageData = new PageImageDataModel();
            SessionDataManager SM = new SessionDataManager();
            ConvertModel(SM, UIM, pageData, 1);

            return Content(Url.Content(@"~\Content\" + file.FileName));
                                  
        }
    }
}
