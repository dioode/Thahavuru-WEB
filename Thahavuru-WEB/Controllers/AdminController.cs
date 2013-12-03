using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Thahavuru_WEB.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

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
            return PartialView();
        }
    }
}
