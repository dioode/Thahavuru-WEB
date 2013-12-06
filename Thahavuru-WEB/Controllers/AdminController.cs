using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thahavuru.DataAccessLayer;
using Thahavuru.Resources.ViewModels;
using Thahavuru_WEB.App_Start;
using Thahavuru_WEB.Models;

namespace Thahavuru_WEB.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Header()
        {
            return PartialView();
        }

        public ActionResult FaceRecAdminIndex()
        {
            return PartialView();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }

        public ActionResult AddNewAttribute()
        {


            return View();
        }

        public ActionResult AddNewAttributeDetailed()
        {
            DataAccessSingleton dAS = new DataAccessSingleton();
            List<FaceAttribute> attList = dAS.GetAllAttributes();

            AttributesListModel aLM = new AttributesListModel();
            aLM.AttributeList = new List<AttributeModel>();

            foreach (var att1 in attList)
            {
                AttributeModel att = new AttributeModel();
                att.Name = att1.Name;
                att.AttributeId = att1.AttributeId;
                att.ClassificationTechnique = att1.ClassificationTechnique;
                att.IsBiometric = att1.IsBiometric;
                att.NumberOfClasses = att1.NumberOfClasses;
                foreach (var iC1 in att1.ClassesInOrder)
                {
                    IndClass iC = new IndClass();
                    iC.Name = iC1.Name;
                    iC.ClassNumber = iC1.ClassNumber;
                    iC.Id = iC1.Id;
                    att.IndClasses.Add(iC);
                }
                aLM.AttributeList.Add(att);
            }

            return PartialView(aLM);
        }

        public JsonResult UpdateAttribute(int attId, string attName, string cTechnique, bool isBiometric, object[] indClasses)
        {
            //SessionDataManager SM = new SessionDataManager();
            //SM.Clear(SessionDataManager.Key.pageImageDataModel);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditAttribute(FormCollection collection)
        {
            FaceAttribute fa = new FaceAttribute();
            fa.AttributeId = Convert.ToInt32(collection["attId"].Trim());
            fa.Name = collection["attName"].Trim();
            fa.NumberOfClasses = Convert.ToInt32(collection["nClasses"].Trim());
            fa.ClassificationTechnique = collection["attCTechnique"].Trim();

            fa.IsBiometric = collection["r" + fa.AttributeId] == "on" ? true : false;
            //object radio2 = collection["r" + fa.AttributeId];
            object o = collection["r" + fa.AttributeId];
            for (int i = 0; i < fa.NumberOfClasses; i++)
            {
                IndividualClass iC = new IndividualClass();
                iC.Name = collection["listItemName" + fa.AttributeId.ToString() + (i + 1).ToString()];
                iC.ClassNumber = Convert.ToInt32(collection["listClassNumber" + fa.AttributeId.ToString() + (i + 1).ToString()]);//listClassNumber
                iC.Id = Convert.ToInt32(collection["listId" + fa.AttributeId.ToString() + (i + 1).ToString()]);
                fa.ClassesInOrder.Add(iC);
            }

            return RedirectToAction("AddNewAttribute");
        }

        [HttpPost]
        public ActionResult DeleteAttribute(int attId)
        {


            return RedirectToAction("AddNewAttribute");
        }

        [HttpPost]
        public ActionResult AddNewAttribute(FormCollection collection)
        {


            return RedirectToAction("AddNewAttribute");
        }
    }
}
