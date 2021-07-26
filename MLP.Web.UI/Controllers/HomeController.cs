using MLP.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace MLP.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

          
            //var props = new Dictionary<string, Type>() {
            //    { "Title", typeof(string) },
            //    { "Text", typeof(string) },
            //    { "Tags", typeof(string[]) }
            //};
            //dynamic x = new Test(props);
            //var finalData = x.Title;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}