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
using System.Data.SqlClient;
using System.Data;
using System.Net;


namespace MLP.Web.UI.Controllers
{
    public class InvoicesController : Controller
    {
        private MLPDB01Entities db = new MLPDB01Entities();
        UnitOfWork unitofwork = new UnitOfWork();

        [Authorize]
        public ActionResult Index()
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
            return View();

        }

        public ActionResult Getinvoicedata(string CustCode)
        {
            List<InvoiceViewModel> InvoicesListVM = new List<InvoiceViewModel>();
            var InvoiceList = unitofwork.Invoices.GetWhere(b => b.FK_CustomerCode == CustCode || b.FK_CarCode==CustCode).ToList();

            foreach (var item in InvoiceList)
            {
                InvoiceViewModel InvoiceObj = new InvoiceViewModel();

                var CustomerObj = unitofwork.customer.GetWhere(s => s.CustomerCode == item.FK_CustomerCode).FirstOrDefault();
                var CarNumber = unitofwork.Vehicles.GetWhere(x => x.CarNumber == item.FK_CarCode).Select(x => x.CarNumber).FirstOrDefault();
                var totalReturn = unitofwork.ReturnInvoices.GetWhere(x => x.FK_InvoiceNo == item.InvoiceNo).Select(s => s.TotalRetAmount).FirstOrDefault() ?? 0;

                InvoiceObj.ID = item.ID;
                InvoiceObj.CustomerName = $"{CustomerObj.FirstName} {CustomerObj.LastName}";
                InvoiceObj.Mobil = CustomerObj.Mobile;
                InvoiceObj.InvoiceNo = item.InvoiceNo;
                InvoiceObj.InvoiceDate = item.InvoiceDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt");
                InvoiceObj.CarNumber = CarNumber ?? string.Empty;
                InvoiceObj.TotalAmount = item.TotalAmount ?? 0;
                InvoiceObj.VatPercent = item.VatPercent ?? 0;
                InvoiceObj.Vat = item.Vat ?? 0;
                InvoiceObj.DiscountAmount = item.DiscountAmount ?? 0;
                InvoiceObj.TotalWithVat = item.TotalWithVat - totalReturn;
                InvoiceObj.Cash = (unitofwork.InvoicePayments.GetWhere(x => x.FK_InvoiceID == item.ID && x.FK_PaymentTypeID == 1 && x.FK_InvoiceTypeID == 1).Select(s => s.Total).FirstOrDefault()) ?? 0;
                InvoiceObj.Visa = (unitofwork.InvoicePayments.GetWhere(x => x.FK_InvoiceID == item.ID && x.FK_PaymentTypeID == 2 && x.FK_InvoiceTypeID == 1).Select(s => s.Total).FirstOrDefault()) ?? 0;
                InvoiceObj.Credit = (unitofwork.InvoicePayments.GetWhere(x => x.FK_InvoiceID == item.ID && x.FK_PaymentTypeID == 3 && x.FK_InvoiceTypeID == 1).Select(s => s.Total).FirstOrDefault()) ?? 0;
                InvoiceObj.TotalReturn = totalReturn;
                InvoiceObj.CashReturn = (unitofwork.InvoicePayments.GetWhere(x => x.FK_InvoiceID == item.ID && x.FK_PaymentTypeID == 1 && x.FK_InvoiceTypeID == 2).Select(s => s.Total).FirstOrDefault()) ?? 0;
                InvoiceObj.VisaReturn = (unitofwork.InvoicePayments.GetWhere(x => x.FK_InvoiceID == item.ID && x.FK_PaymentTypeID == 2 && x.FK_InvoiceTypeID == 2).Select(s => s.Total).FirstOrDefault()) ?? 0;
                InvoiceObj.CreditReturn = (unitofwork.InvoicePayments.GetWhere(x => x.FK_InvoiceID == item.ID && x.FK_PaymentTypeID == 3 && x.FK_InvoiceTypeID == 2).Select(s => s.Total).FirstOrDefault()) ?? 0;
                InvoiceObj.UserName = $"{item.User.FirstName} {item.User.LastName}";
                InvoiceObj.ServiceCenterName = (unitofwork.ServiceCenter.GetWhere(s => s.ID == item.FK_ServiceCenterID).Select(s => s.CenterName).FirstOrDefault());
                InvoicesListVM.Add(InvoiceObj);
            }

            return Json(InvoicesListVM, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetInvoiceDataDetails(int InvoiceID)
        {
            List<InvoiceDetailsViewModel> InvoicesDetailsVM = new List<InvoiceDetailsViewModel>();

            var InvoiceDetails = unitofwork.InvoiceDetails.GetWhere(b => b.FK_InvoiceID == InvoiceID).OrderByDescending(s=>s.Cost).ToList();

            foreach (var item in InvoiceDetails)
            {
                InvoiceDetailsViewModel InvoiceDetailsObj = new InvoiceDetailsViewModel();

                var ItemObj = unitofwork.SalesItem.GetWhere(s => s.ID == item.FK_SalesItemID).FirstOrDefault();
                if (ItemObj !=null)
                {
                    InvoiceDetailsObj.ID = item.ID;
                    InvoiceDetailsObj.ItemCode = ItemObj.ItemCode;
                    InvoiceDetailsObj.ItemName = ItemObj.ItemName;
                    InvoiceDetailsObj.Qty = item.Qty;
                    //InvoiceDetailsObj.OriginalPrice = item.OriginalPrice;
                    InvoiceDetailsObj.Price = item.Price;
                    //InvoiceDetailsObj.Cost = item.Cost;
                    InvoiceDetailsObj.IsPackage = item.FK_BundleID == null ? "" :  "<i class='fa fa-star'></i>";

                    InvoicesDetailsVM.Add(InvoiceDetailsObj);
                }
                
            }

            return Json(InvoicesDetailsVM, JsonRequestBehavior.AllowGet);
        }


        public class dataitems
        {
            public dataitems()
            {
                _items = new List<item>();
            }
            public data _data { get; set; }
            public List<item> _items { get; set; }
        }

        public class item
        {
            public int InvItemId { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public int Qty { get; set; }
            public decimal Price { get; set; }
            public int ItmReturn { get; set; }
        }

        public ActionResult Print(int InvoiceID)
        {
            var Lang = new SqlParameter("@Lang", "en-US");
            var ManagerID = new SqlParameter("@ManagerID", 10);
            var InvId = new SqlParameter("@InvId", InvoiceID);
            var pOSInvoices = db.Database
                               .SqlQuery<data>("SiteManager_Invoices_Print @Lang,@ManagerID,@InvId", Lang, ManagerID, InvId).First();

            var Lang2 = new SqlParameter("@Lang", "en-US");
            var InvId2 = new SqlParameter("@InvoiceId", InvoiceID);
            var _PosItems = db.Database
                               .SqlQuery<item>("SiteManager_Invoice_GetItemsAndBundle @Lang,@InvoiceId", Lang2, InvId2).ToList();

            var _dataItems = new dataitems();
            _dataItems._data = pOSInvoices;
            _dataItems._items = _PosItems;
            return View(_dataItems);
        }
    }
}
