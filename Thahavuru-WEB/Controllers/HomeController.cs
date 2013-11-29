using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thahavuru_WEB.App_Start;
using Thahavuru_WEB.Models;
using System.IO;
using Thahavuru.Techniques.WebServiceMethods;
using Thahavuru.Resources.ViewModels;
using Emgu.CV;
using Emgu.CV.Structure;

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
                    FaceMatchAdapter adapter = new FaceMatchAdapter();
                    UIM.PageNumber = nextPage;
                    adapter.FaceMatch(ref UIM);
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

        //[AsyncTimeout(30000)]
        //public ActionResult Upload(HttpPostedFileBase FileData, FormCollection forms)
        //{
                
        //    var file = Request.Files["Filedata"];
               
        //    string savePath = Server.MapPath(@"~\Content\" + file.FileName);
        //    file.SaveAs(savePath);
               

        //    UserInterfaceModel  model = new UserInterfaceModel();
        //    model.PageNumber = 1;
        //    model.SearchingPerson.FaceofP.FaceImage = new Image<Gray, byte>(savePath);
        //    FaceMatchAdapter adapter = new FaceMatchAdapter();

        //    adapter.FaceMatch(ref model);

        //    PageImageDataModel pageData = new PageImageDataModel();
        //    SessionDataManager SM = new SessionDataManager();
        //    ConvertModel(SM, model, pageData, 1);

        //    return Content(Url.Content(@"~\Content\" + file.FileName));
                                  
        //}



        public ActionResult Upload(HttpPostedFileBase FileData, FormCollection forms)
        {
            var file = Request.Files["Filedata"];
            string savePath = Server.MapPath(@"~\Content\" + file.FileName);
            file.SaveAs(savePath);
            return Content(Url.Content(@"~\Content\" + file.FileName));
        }

        //[AsyncTimeout(30000)]
        public JsonResult Recognize(string data)
        {
            string savePath = Server.MapPath(@data);


            UserInterfaceModel model = new UserInterfaceModel();
            model.PageNumber = 1;
            model.SearchingPerson.FaceofP.FaceImage = new Image<Gray, byte>(savePath);
            FaceMatchAdapter adapter = new FaceMatchAdapter();

            adapter.FaceMatch(ref model);

            PageImageDataModel pageData = new PageImageDataModel();
            SessionDataManager SM = new SessionDataManager();
            ConvertModel(SM, model, pageData, 1);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
