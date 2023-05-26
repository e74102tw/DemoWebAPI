using DemoWebAPI.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = string.Format("{0}", "DemoWebAPI");
            ViewBag.Version = Common.AssemblyVersion;
            return View();
        }
    }
}
