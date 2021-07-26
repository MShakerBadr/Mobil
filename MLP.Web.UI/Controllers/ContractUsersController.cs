using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLP.DAL;
using MLP.Web.UI.Models;
using System.Web.Security;

namespace MLP.Web.UI.Controllers
{
    public class ContractUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult Create()
        {
            int userid = GetUserId();
          ViewBag.AllContracts = new SelectList(db.Contracts.Where(s => s.ContractUsers.Where(p => p.FK_UserID == userid).Count() > 0).ToList(), "ID", "Corporate");
            return View();
        }

        // POST: ContractUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        public JsonResult SaveData(ContractUserViewModel data)
        {
            //var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (authCookie == null || authCookie.Value == null)
            //{
            //  RedirectToAction("Login", "Account",null);
            //    return Json(false, JsonRequestBehavior.AllowGet);
            //}
            try
            {
                ContractCar car = new ContractCar();
                car.FK_ContractID = data.ID;
                car.EmpNo = data.Empno;
                car.CreatorType = 2;
                car.CreatorID = GetUserId();
                car.CreationDate = DateTime.Now;
                car.LastModifiedDate = DateTime.Now;
                car.IsBlocked = false;
                car.IsActive = true;
                car.CarCode = data.Carno.ToString() + " " + data.Carchar.ToUpper().ToString();

                db.ContractCars.Add(car);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
           
           

        }

        [HttpGet]
        public JsonResult Checkcar(int Carno,string Carchar)
        {
            string carnumber = Carno.ToString() + " " + Carchar.ToUpper().ToString();
            if (db.ContractCars.Count(s=>s.CarCode==carnumber)>0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        public int GetUserId()
        {
            string UserObj = "";
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && authCookie.Value != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && authTicket.UserData != null)
                {
                    UserObj = authTicket.UserData;
                }
            }
            string[] UserData = UserObj.Split('|');
          return Convert.ToInt32(UserData[0]);
        }

    }
       
}
