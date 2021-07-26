using MLP.BAL;
using MLP.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using Newtonsoft.Json;
using MLP.DAL;
using System.Data.Entity;
using MLP.BAL.ViewModels;

namespace MLP.Web.UI.Controllers
{
    public class RequestOrderController : Controller
    {

        UnitOfWork unitofwork = new UnitOfWork();
        // GET: Request
        public ActionResult Index(int? ID)
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
            if (UserObj == "")
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "" });
            }

            string[] UserData = UserObj.Split('|');
            int ServiceCenterID = Int32.Parse(UserData[3]);
            int[] NotAcceptence = { 2, 3, 4 };
            if (ID != null)
            {
                var CurrentRequest = unitofwork.RequesOrder.GetWhere(s => s.ID == ID).FirstOrDefault();
                if (CurrentRequest != null)
                {
                    if (CurrentRequest.FK_StatusID == 1)
                    {
                        CurrentRequest.FK_StatusID = 4;
                        CurrentRequest.LastModifiedDate = DateTime.Now;
                    }

                    unitofwork.Commit();
                    return RedirectToAction("Index");
                }
               
            }

            //////////////////

            var RequestOrderViewModelList = unitofwork.RequesOrder.GetAllRequesOrder(ServiceCenterID);


            return View(RequestOrderViewModelList);
        }
        [HttpGet]
        public ActionResult Create(int? ID)
        {
            var Request = unitofwork.RequesOrder.GetWhere(s => s.ID == ID).FirstOrDefault();
            if (Request != null)
            {
                if (Request.FK_StatusID!=1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.SalesItemList = unitofwork.SalesItem.GetAllSalesItem();
                    var RequestItemsList = ID == null ? new List<RequestItemsViewModel>() : unitofwork.RequesItems.GetAllRequestItemsList(ID);
                    return View(RequestItemsList);
                }
            }
            else
            {
                ViewBag.SalesItemList = unitofwork.SalesItem.GetAllSalesItem();
                var RequestItemsList = ID == null ? new List<RequestItemsViewModel>() : unitofwork.RequesItems.GetAllRequestItemsList(ID);
                return View(RequestItemsList);
            }

        }
        [HttpPost]
        public ActionResult Create(Data data)
        {
            string FK_RequestNo = "";
            if (data.ID ==0)//create
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
                if (UserObj == "")
                {
                    return RedirectToAction("Login", "Account", new { returnUrl = "" });
                }

                string[] UserData = UserObj.Split('|');
                int ServiceCenterID = Int32.Parse(UserData[3]);
                int[] NotAcceptence = { 2, 3, 4 };


               
                RequestOrder ROrder = new RequestOrder();
                ROrder.FK_CenterID = ServiceCenterID;
                ROrder.FK_UserID = int.Parse(UserData[0]);
                ROrder.FK_RequestType = 1;
                ROrder.FK_StatusID = 1;
                ROrder.CreationDate = DateTime.Now;
                ROrder.RequestDate = DateTime.Now ;
                ROrder.FK_RequestNo = FK_RequestNo;
                unitofwork.RequesOrder.Insert(ROrder);
                unitofwork.Commit();
                var LastOne = unitofwork.RequesOrder.GetAll().OrderByDescending(u => u.ID).FirstOrDefault();
                FK_RequestNo = ServiceCenterID.ToString() + "-" + (LastOne.ID).ToString();
                LastOne.FK_RequestNo = FK_RequestNo;
                unitofwork.Commit();
            }
            else
            {
                RequestOrder RO = unitofwork.RequesOrder.GetById(data.ID.Value);
                var RemovedList= unitofwork.RequesItems.GetAll().Where(s => s.FK_RequestNo==RO.FK_RequestNo).ToList();
                unitofwork.RequesItems.RemoveRange(RemovedList);
                FK_RequestNo = RO.FK_RequestNo;
                RO.LastModifiedDate = DateTime.Now;
                unitofwork.RequesOrder.Update(RO);
                unitofwork.Commit();

            }


            List<RequestItem> RequestItemList = new List<RequestItem>();
            foreach (var item in data.List)
            {
                RequestItem RI = new RequestItem();
                RI.FK_SalesItemID = item.SalesItemID;
                RI.FK_RequestNo = FK_RequestNo;
                RI.Quantity = item.Quantity;
                RequestItemList.Add(RI);
            }
            unitofwork.RequesItems.InsertList(RequestItemList);

            unitofwork.Commit();

            return Json(true, JsonRequestBehavior.AllowGet);

        }
    }
   public class Data
    {
       
        public List<RequestItemsViewModel> List { get; set; }
        public int? ID { get; set; }
    }
}