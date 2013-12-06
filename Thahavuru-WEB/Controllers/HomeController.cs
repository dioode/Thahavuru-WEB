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
using Thahavuru.DataAccessLayer;

namespace Test_Web.Controllers
{
    public class HomeController : Controller
    {
        #region Action Methods

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Partial View Action

        public ActionResult Header()
        {
            return PartialView();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }

        public ActionResult FaceRecognizerIndex()
        {
            return PartialView();
        }

        public ActionResult AttributeTable()
        {
            SessionDataManager SM = new SessionDataManager();

            if (SM.IsExists(SessionDataManager.Key.pageImageDataModel) && SM.Get(SessionDataManager.Key.pageImageDataModel) != null)
            {
                PageImageDataModel Pagedata = (PageImageDataModel)SM.Get(SessionDataManager.Key.pageImageDataModel);
                return PartialView(Pagedata);
            }
            return PartialView(null);
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

        public ActionResult Upload(HttpPostedFileBase FileData, FormCollection forms)
        {
            var file = Request.Files["Filedata"];
            string savePath = Server.MapPath(@"~\Content\" + file.FileName);
            file.SaveAs(savePath);
            return Content(Url.Content(@"~\Content\" + file.FileName));
        }

        #endregion

        #region Json Requests

        public JsonResult ClearResults(object data)
        {
            SessionDataManager SM = new SessionDataManager();
            SM.Clear(SessionDataManager.Key.pageImageDataModel);
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
                    ConvertModel(SM, UIM, pageData, previousPage);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Recognize(string imageName, string userName)
        {
            string savePath = Server.MapPath(imageName);

            UserInterfaceModel model = new UserInterfaceModel();
            model.PageNumber = 1;
            model.SearchingPerson.FaceofP.FaceImage = new Image<Gray, byte>(savePath);
            FaceMatchAdapter adapter = new FaceMatchAdapter();

            adapter.FaceMatch(ref model);

            PageImageDataModel pageData = new PageImageDataModel();
            pageData.AdvancedSearchUserName = userName;
            SessionDataManager SM = new SessionDataManager();
            ConvertModel(SM, model, pageData, 1);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Private Methods

        private static void ConvertModel(SessionDataManager SM, UserInterfaceModel UIM, PageImageDataModel pageData, int page)
        {
            pageData.ImageList = new List<ImageModel>();
            if (UIM.SearchingPerson.MatchedFaceIdSet[page] != null)
            {
                for (int i = 0; i < UIM.SearchingPerson.MatchedFaceIdSet[page].Count; i++)
                {
                    ImageModel img = new ImageModel();
                    img.MatchedImageURI = @"http://localhost/imgs/User%20(" + UIM.SearchingPerson.MatchedFaceIdSet[page][i] + ").jpg";
                    img.UserName = "User (" + UIM.SearchingPerson.MatchedFaceIdSet[page][i] + ")";
                    img.Address = "Address (" + UIM.SearchingPerson.MatchedFaceIdSet[page][i] + ")";
                    pageData.ImageList.Add(img);
                }
            }

            if (pageData.AdvancedSearchUserName != null && pageData.AdvancedSearchUserName.Trim() != "")
            {
                DataAccessSingleton dAS = new DataAccessSingleton();
                List<PersonVM> persons = dAS.GetPersonByName(pageData.AdvancedSearchUserName);
                if (persons != null && persons.Count != 0)
                {
                    PersonVM person = persons.First();
                    ImageModel img = new ImageModel();
                    img.MatchedImageURI = @"http://localhost/imgs/User%20(" + person.Id + ").jpg";
                    img.UserName = person.Name;
                    img.Address = person.Address;
                    pageData.AdvancedSearchResult = img;
                }
                else
                {
                    pageData.AdvancedSearchResult = null;
                }
            }
            else
            {
                pageData.AdvancedSearchResult = null;
            }

            pageData.attributeValues = new List<KeyValuePair<string, string>>();
            foreach (var item in UIM.SearchingPerson.SearchTrakKeeper[UIM.PageNumber - 1])
            {
                var atribute = UIM.SearchingPerson.FaceofP.FaceAttributes[item[0] - 1];
                pageData.attributeValues.Add(new KeyValuePair<string, string>(atribute.Name, atribute.ClassesInOrder.Where(x => x.ClassNumber == item[1]).First().Name));
            }

            pageData.PageNumber = page;
            pageData.Next = pageData.PageNumber == (UIM.MaxLeaves) ? false : true;
            pageData.Back = pageData.PageNumber == 1 ? false : true;

            SM.Clear(SessionDataManager.Key.pageImageDataModel);
            SM.Clear(SessionDataManager.Key.userInterfaceModel);

            SM.Set(SessionDataManager.Key.pageImageDataModel, pageData);
            SM.Set(SessionDataManager.Key.userInterfaceModel, UIM);
        }

        #endregion

    }
}
