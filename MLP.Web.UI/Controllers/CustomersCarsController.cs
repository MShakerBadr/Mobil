using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLP.DAL;
using MLP.BAL;

namespace MLP.Web.UI.Controllers
{
    public class CustomersCarsController : Controller
    {
        private UnitOfWork unitofwork = new UnitOfWork();

        // GET: CustomersCars
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Checkcar(string Carno,string Carchar)
        {
           var Data= unitofwork.Vehicles.GetAll().FirstOrDefault(s => s.CarNumber == Carno + " " + Carchar.ToUpper() );
            if (Data == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            Data.LastModifiedDate = DateTime.Now;
            unitofwork.Commit();
            var Customer = unitofwork.customer.GetAll().Where(r => r.Mobile == Data.FK_CustomerCode)
                .Select(s=>new {Name= s.FirstName+" "+s.LastName,Mobile=s.Mobile,MobileUser=s.IsMobileUser,CreationDate=s.CreationDate } ).FirstOrDefault();
            return Json(Customer, JsonRequestBehavior.AllowGet);
        }



    }
}
