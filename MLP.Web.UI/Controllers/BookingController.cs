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

namespace MLP.Web.UI.Controllers
{
    public class BookingController : Controller
    {
        UnitOfWork unitofwork = new UnitOfWork();
        //
        // GET: /Booking/
        [Authorize]
        public ActionResult Index()
        {
            //string currentUserId = User.Identity.GetUserId();

            //ApplicationDbContext context = new ApplicationDbContext();
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

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
              return  RedirectToAction("Login", "Account", new { returnUrl = "" });
            }

                string[] UserData = UserObj.Split('|');
                int ServiceCenterID = Int32.Parse(UserData[3]);
                int[] NotAcceptence = { 2, 3, 4 };
            
            #region Change All Booking Status
            //Change Booking status who is Planned and the customer left his date
            var AllBookingList = unitofwork.Booking.GetWhere(b => b.FK_BookingStatusID == 1 && DbFunctions.TruncateTime(b.BookingDate) < DbFunctions.TruncateTime(DateTime.Now)).ToList();


            foreach (var item in AllBookingList)
            {
                var CustomerObj = unitofwork.customer.GetWhere(c => c.ID == item.FK_CustomerID).FirstOrDefault();
                var InvoicesList = unitofwork.Invoices.GetWhere(i => i.FK_CustomerCode == CustomerObj.CustomerCode).Select(i => DbFunctions.TruncateTime(i.InvoiceDate)).ToList();

                if (InvoicesList.Contains(item.BookingDate.Value.Date) && (item.BookingDate.Value.Date == DateTime.Now.Date))
                    item.FK_BookingStatusID = 1;

                else if (item.BookingDate.Value.Date < DateTime.Now.Date)
                    item.FK_BookingStatusID = 2;

                item.LastModifiedDate = DateTime.Now;
            }
            unitofwork.Commit();
            #endregion

            var CancelReasons = unitofwork.BookingCancelReasons.GetWhere(c => c.IsMobilePortal == true).ToList();
            ViewBag.CancelReasonList = new SelectList(CancelReasons, "ID", "Name");

            var BookingList = unitofwork.Booking.GetWhere(b => b.FK_ServiceCenterID == ServiceCenterID && !NotAcceptence.Contains(b.FK_BookingStatusID.Value)).ToList();
            List<BookingViewModel> BookingVmList = new List<BookingViewModel>();

            foreach (var item in BookingList)
            {
                BookingViewModel BookingObj = new BookingViewModel();
                var CustomerObj = unitofwork.customer.GetById(item.FK_CustomerID.Value);
                var CarNumber = unitofwork.Vehicles.GetWhere(x => x.CarNumber == item.Car_Code).Select(x => x.CarNumber).FirstOrDefault();

                string Serivces = "";
                var BookingSerivcesIds = unitofwork.BookingServices.GetWhere(x => x.FK_BookingID == item.ID).Select(x => x.FK_ServiceID).ToList();
                var BookingSerivcesList = unitofwork.SalesItem.GetWhere(s => BookingSerivcesIds.Contains(s.ID));

                foreach (var Serviceitem in BookingSerivcesList)
                {
                    Serivces = Serivces + "," + Serviceitem.ItemName;
                }

                string FinalSerivces = Serivces.Remove(0, 1);

                var BookingSlotsIds = unitofwork.BookingTimeSlots.GetWhere(x => x.FK_BookingID == item.ID).Select(x => x.FK_TimeSlotID).ToList();
                var BookingSlotsList = unitofwork.TimeSlots.GetWhere(t => BookingSlotsIds.Contains(t.ID)).ToList();

                BookingObj.ID = item.ID;
                BookingObj.CustomerName = CustomerObj.FirstName + " " + CustomerObj.LastName;
                BookingObj.CustomerMobile = CustomerObj.Mobile;
                BookingObj.CarNumber = CarNumber ?? string.Empty;
                BookingObj.Services = FinalSerivces;
                BookingObj.BookingDate = item.BookingDate.Value.ToString("dd/MM/yyyy");
                BookingObj.FromTime = BookingSlotsList.FirstOrDefault().StartTime.Value.Hours.ToString("00.##") + ":" + BookingSlotsList.FirstOrDefault().StartTime.Value.Minutes.ToString("00.##");
                BookingObj.ToTime = BookingSlotsList.LastOrDefault().EndTime.Value.Hours.ToString("00.##") + ":" + BookingSlotsList.LastOrDefault().EndTime.Value.Minutes.ToString("00.##");
                BookingObj.BookingStatus = unitofwork.BookingStatus.GetById(item.FK_BookingStatusID.Value).Name;
                BookingVmList.Add(BookingObj);

            }
            return View(BookingVmList);
        }
        [Authorize]
        public ActionResult BookingCancel(int Id, int reasonId)
        {
            var BookingObj = unitofwork.Booking.GetById(Id);
            if (BookingObj != null)
            {
                BookingObj.FK_BookingStatusID = 4;
                BookingObj.FK_CancelReasonID = reasonId;
                BookingObj.IsUserCancel = false;
                BookingObj.LastModifiedDate = DateTime.Now;
                unitofwork.Commit();
            }
            return RedirectToAction("Index");
        }
    }
}