using MLP.API.Utilities;
using MLP.BAL;
using MLP.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MLP.DAL;

namespace MLP.API.Controllers
{
    public class BookingController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();
        // GET api/booking
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/booking/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        public BookingHistoryResponse GetHistory(string token, string lang)
        {
            BookingHistoryResponse resp = new BookingHistoryResponse();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);
                if (checktoken == true)
                {
                    resp.error = 0;
                    resp.message = "Success";

                    var CustomerObj = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.Customer).FirstOrDefault();

                    #region Change All Booking Status

                    //Change Booking status who is Planned and the customer left his date
                    var AllBookingList = unitofwork.Booking.GetWhere(b => b.FK_BookingStatusID == 1 && DbFunctions.TruncateTime(b.BookingDate) <= DbFunctions.TruncateTime(DateTime.Now)).ToList();
                    var InvoicesList = unitofwork.Invoices.GetWhere(i => i.FK_CustomerCode == CustomerObj.CustomerCode).Select(i => DbFunctions.TruncateTime(i.InvoiceDate)).ToList();
                    foreach (var item in AllBookingList)
                    {
                        if (InvoicesList.Contains(item.BookingDate.Value.Date))
                        {
                            item.FK_BookingStatusID = 5;
                            unitofwork.Commit();
                            if (item.BookingDate.Value.Date == DateTime.Now.Date)
                            {
                                var UserFireBaseToken = unitofwork.MLPTokenlst.GetAll().FirstOrDefault(s => s.UserToken == CustomerObj.CustomerCode);
                                if (UserFireBaseToken != null)
                                {
                                    var Msg = "Reminder to fill your service evaluation";
                                    MLPNotification newNotification = new MLPNotification();
                                    //add
                                    newNotification.NotificationMessage = Msg;
                                    newNotification.FK_NotificationTypeID = 6;
                                    newNotification.IsActive = true;
                                    newNotification.LastModifiedDate = DateTime.Now;
                                    newNotification.CreationDate = DateTime.Now;
                                    newNotification.NotificationDate = DateTime.Now;
                                    newNotification.CreatorID = 10;
                                    unitofwork.Notifications.Insert(newNotification);
                                    unitofwork.Commit();
                                    unitofwork.CustomerNotification.Insert(new CustomerNotification() { FK_CustomerID = CustomerObj.ID, IsSeen = false, FK_NotificationID = unitofwork.Notifications.GetAll().OrderByDescending(s => s.ID).FirstOrDefault().ID });
                                    unitofwork.Commit();
                                    SendPushNotification(Msg, "Evaluation", UserFireBaseToken.FireBaseToken);
                                }
                            }
                        
                           
                        }
                        else if (item.BookingDate.Value.Date < DateTime.Now.Date)
                        {
                            item.FK_BookingStatusID = 2;
                        }

                        item.LastModifiedDate = DateTime.Now;
                    }
                    unitofwork.Commit();
                    #endregion

                    var BookingList = unitofwork.Booking.GetWhere(b => b.FK_CustomerID == CustomerObj.ID).OrderByDescending(b => b.FK_BookingStatusID).OrderByDescending(b => b.BookingDate).ToList();
                    resp.data = new List<BookingData>();
                    foreach (var item in BookingList)
                    {
                        BookingData BookingObj = new BookingData();

                        var ServiceCenter = unitofwork.ServiceCenter.GetWhere(x => x.IsActive == true && x.ID == item.FK_ServiceCenterID).FirstOrDefault();
                        var ServiceCenterName = "";
                        var ServiceCenterId = 0;
                        if (ServiceCenter == null)
                        {
                            ServiceCenterId = 0;
                            ServiceCenterName = "";
                        }
                        else
                        {
                            ServiceCenterId = ServiceCenter.ID;
                            if (lang == "ar")
                                ServiceCenterName = ServiceCenter.CenterNameAr;
                            else
                                ServiceCenterName = ServiceCenter.CenterName;
                        }
                        var car = unitofwork.Vehicles.GetWhere(x => x.CarNumber == item.Car_Code).FirstOrDefault();
                        var CarNumber = "";
                        var CarId = 0;
                        if (car == null)
                        {
                            CarNumber = "";
                            CarId = 0;
                        }
                        else
                        {
                            CarNumber = car.CarNumber;
                            CarId = car.ID;
                        }
                        string Serivces = "";
                        var BookingSerivcesIds = unitofwork.BookingServices.GetWhere(x => x.FK_BookingID == item.ID).Select(x => x.FK_ServiceID).ToList();


                        var BookingSerivcesList = unitofwork.SalesItem.GetWhere(s => BookingSerivcesIds.Contains(s.ID));


                        List<int> ServicesIDs = new List<int>();
                        foreach (var Serviceitem in BookingSerivcesList)
                        {
                            ServicesIDs.Add(Serviceitem.ID);
                            if (lang == "ar")
                                Serivces = Serivces + "," + Serviceitem.ItemNameAr;
                            else
                                Serivces = Serivces + "," + Serviceitem.ItemName;
                        }
                        string FinalSerivces = Serivces.Remove(0, 1);

                        var BookingSlotsIds = unitofwork.BookingTimeSlots.GetWhere(x => x.FK_BookingID == item.ID).Select(x => x.FK_TimeSlotID).ToList();
                        var BookingSlotsList = unitofwork.TimeSlots.GetWhere(t => BookingSlotsIds.Contains(t.ID)).ToList();

                        BookingObj.ID = item.ID;
                        BookingObj.ServiceCenterName = ServiceCenterName ?? string.Empty;
                        BookingObj.ServiceCenterID = ServiceCenterId;
                        BookingObj.CarId = CarId;
                        BookingObj.CarNumber = CarNumber ?? string.Empty;
                        BookingObj.Services = FinalSerivces;
                        BookingObj.ServicesID = ServicesIDs;
                        BookingObj.BookingDate = item.BookingDate.Value.ToString("dd/MM/yyyy");
                        // Check IF it's Real Interval Time Id

                        BookingObj.FromTime = BookingSlotsList.FirstOrDefault().StartTime.Value.Hours.ToString("00.##") + ":" + BookingSlotsList.FirstOrDefault().StartTime.Value.Minutes.ToString("00.##");
                        BookingObj.ToTime = BookingSlotsList.LastOrDefault().EndTime.Value.Hours.ToString("00.##") + ":" + BookingSlotsList.LastOrDefault().EndTime.Value.Minutes.ToString("00.##");
                        if (lang == "ar")
                            BookingObj.Status = unitofwork.BookingStatus.GetById(item.FK_BookingStatusID.Value).NameAR;
                        else
                            BookingObj.Status = unitofwork.BookingStatus.GetById(item.FK_BookingStatusID.Value).Name;
                        BookingObj.StatusID = item.FK_BookingStatusID.Value;
                        if (item.BookingDate >= DateTime.Now.AddHours(23))
                            BookingObj.Editable = true;
                        else
                            BookingObj.Editable = false;

                        resp.data.Add(BookingObj);

                    }
                }
                else
                {
                    resp.error = 2;
                    resp.message = "Login again";
                }
            }
            catch (Exception ex)
            {
                resp.error = 1;
                resp.message = ex.Message + " " + ex.InnerException;
                //resp.message = "Check internet connection";
            }
            return resp;
        }

        // POST api/booking
        [HttpPost]
        public AvailableTimesResponse AvailableTimes([FromBody]AvailableTimesBody body)
        {
            AvailableTimesResponse resp = new AvailableTimesResponse();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(body.token);
                if (checktoken == true)
                {
                    resp.error = 0;
                    resp.message = "Success";
                    resp.data = new List<TimeSlots>();

                    TimeSpan TimeSlot = new TimeSpan();
                    DateTime Today = new DateTime();
                    int Day = DateTime.Now.Day;

                    int[] NotAcceptence = { 2, 3, 4 };

                    DateTime ChoosenDate = Convert.ToDateTime(body.ChoosenDate);

                    var ServiceCenter = unitofwork.ServiceCenter.GetWhere(s => s.IsActive == true && s.ID == body.ServiceCenterID).FirstOrDefault();

                    var ServicesList = unitofwork.SalesItem.GetWhere(s => body.Services.Contains(s.ID)).ToList();

                    int Interval = ServicesList.Max(s => s.NoOfServiceTimeSlots.Value) - 1;

                    if (ServiceCenter.OpeningTime == null || ServiceCenter.ClosingTime == null)
                    {
                        resp.error = 4;
                        resp.message = "The service center has no openning and closig time";
                    }
                    else
                    {
                        TimeSpan? difference = ServiceCenter.OpeningTime - ServiceCenter.ClosingTime;
                        int hours = difference.Value.Hours;

                        //// Get the Id of Begining timeslots
                        int FirstId = unitofwork.TimeSlots.GetWhere(t => t.StartTime == ServiceCenter.OpeningTime).Select(t => t.ID).FirstOrDefault();

                        int EndId = unitofwork.TimeSlots.GetWhere(t => t.EndTime == ServiceCenter.ClosingTime).Select(t => t.ID).FirstOrDefault();

                        var Times = unitofwork.TimeSlots.GetWhere(t => t.ID >= FirstId && t.ID <= EndId).ToList();

                        if (hours == 0) //24 hrs
                        {
                            hours = 24;
                            // Get the timeslots
                            Times = unitofwork.TimeSlots.GetWhere(t => t.ID >= FirstId && t.ID <= (hours * 2)).Union(unitofwork.TimeSlots.GetWhere(t => t.ID <= EndId)).ToList();
                        }

                        if (ServiceCenter.ClosingTime < ServiceCenter.OpeningTime && (ServiceCenter.ClosingTime.ToString() != "00:00:00")) // ex: 9:00 am to 8:00 am
                        {
                            Times = unitofwork.TimeSlots.GetWhere(t => t.ID >= FirstId).Union(unitofwork.TimeSlots.GetWhere(t => t.ID <= EndId)).ToList();
                        }

                        var BookingList = unitofwork.Booking.GetWhere(B => DbFunctions.TruncateTime(B.BookingDate) == DbFunctions.TruncateTime(ChoosenDate) && B.FK_ServiceCenterID == body.ServiceCenterID && !NotAcceptence.Contains(B.FK_BookingStatusID.Value)).Select(B => B.ID).ToList();

                        if (BookingList != null)
                        {
                            var BookingSlots = unitofwork.BookingTimeSlots.GetWhere(BS => BookingList.Contains(BS.FK_BookingID.Value)).Select(BS => BS.FK_TimeSlotID).ToList();
                            var ShortlistedTimes = unitofwork.TimeSlots.GetWhere(x => BookingSlots.Contains(x.ID)).ToList();

                            Times = Times.Except(ShortlistedTimes).ToList();
                        }

                        if (ChoosenDate.Date == DateTime.Now.Date)
                        {
                            if (DateTime.Now.Minute < 30 && DateTime.Now.Minute != 0)
                                TimeSlot = new TimeSpan(DateTime.Now.Hour, 30, 0);
                            else
                                TimeSlot = new TimeSpan(DateTime.Now.AddHours(1).Hour, 0, 0);


                            List<TimeSpan> TodayNotAllowed = new List<TimeSpan>();

                            TimeSpan Minutes = new TimeSpan(0, 30, 0);
                            TodayNotAllowed.Add(TimeSlot);
                            for (int i = 0; i < 5; i++)
                            {
                                if (TimeSlot.ToString() != "23:30:00")
                                    TimeSlot = TimeSlot.Add(Minutes);
                                else
                                {
                                    TimeSlot = new TimeSpan(0, 0, 0);
                                    Day = Day + 1;
                                }

                                TodayNotAllowed.Add(TimeSlot);
                                Today.AddMinutes(Minutes.Minutes);
                            }

                            if (Day == ChoosenDate.Day)
                            {
                                var TimesBeforeThird = Times.Where(x => x.StartTime < TodayNotAllowed[5]).Select(x => x.StartTime).ToList();
                                Times = Times.Where(x => !TodayNotAllowed.Contains(x.StartTime.Value) && !TimesBeforeThird.Contains(x.StartTime.Value)).ToList();
                            }
                            else if (Day > ChoosenDate.Day)
                                Times = Times.Where(x => x.ID == 0).ToList();

                        }

                        if (Times.Count > 0)
                        {
                            for (int i = 0; i < Times.Count; i++)
                            {
                                if ((i + Interval) < Times.Count)
                                {
                                    if (((Times[i + Interval].ID - Times[i].ID) == Interval))
                                    {
                                        TimeSlots TimeObj = new TimeSlots();
                                        TimeObj.FromID = Times[i].ID;
                                        TimeObj.Inetrval = Interval;
                                        TimeObj.TimeFrom = Times[i].StartTime.Value.Hours.ToString("00.##") + ":" + Times[i].StartTime.Value.Minutes.ToString("00.##");
                                        TimeObj.TimeTo = Times[(i + Interval)].EndTime.Value.Hours.ToString("00.##") + ":" + Times[i + Interval].EndTime.Value.Minutes.ToString("00.##");
                                        resp.data.Add(TimeObj);
                                        i = (i + Interval);
                                    }
                                }
                            }
                        }
                        if (resp.data.Count == 0)
                        {
                            resp.error = 3;
                            resp.message = "no times available";
                        }
                    }
                }
                else
                {
                    resp.error = 2;
                    resp.message = "Login again";
                }

            }
            catch (Exception ex)
            {
                resp.error = 1;
                resp.message = "Check internet connection";
            }
            return resp;
        }

        [HttpPost]
        public HttpResponseMessage ConfirmBooking([FromBody]ConfirmBookingBody body)
        {
            MessageObject resp = new MessageObject();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(body.token);
                if (checktoken == true)
                {

                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == body.token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    var customer = unitofwork.customer.GetById(CustomerID);
                    //Check if times are available
                    DAL.Booking BookingObj = new DAL.Booking();
                    BookingObj.FK_CustomerID = CustomerID;
                    BookingObj.FK_ServiceCenterID = body.ServiceCenterID;
                    if (body.CarCode == "-1")
                        BookingObj.Car_Code = null;
                    else
                        BookingObj.Car_Code = body.CarCode;

                    BookingObj.BookingDate = Convert.ToDateTime(body.ChoosenDate);
                    BookingObj.FK_BookingStatusID = 1;
                    BookingObj.LastModifiedDate = DateTime.Now;
                    unitofwork.Booking.Insert(BookingObj);
                    unitofwork.Commit();

                    DAL.BookingService BookingServ = new DAL.BookingService();
                    string services = "";
                    foreach (var item in body.Services)
                    {
                        BookingServ.FK_ServiceID = item;
                        services += "," + (unitofwork.SalesItem.GetById(item).ItemName);
                        BookingServ.FK_BookingID = BookingObj.ID;
                        unitofwork.BookingServices.Insert(BookingServ);
                        unitofwork.Commit();
                    }


                    DAL.BookingTimeSlot BookingTimes = new DAL.BookingTimeSlot();

                    //Insert  Times Slots
                    var SlotsList = unitofwork.TimeSlots.GetWhere(t => t.ID >= body.FromID && t.ID <= (body.FromID + body.Inetrval)).ToList();

                    foreach (var item in SlotsList)
                    {
                        BookingTimes.FK_BookingID = BookingObj.ID;
                        BookingTimes.FK_TimeSlotID = item.ID;
                        unitofwork.BookingTimeSlots.Insert(BookingTimes);
                        unitofwork.Commit();
                    }
                    var ServiceCenter = unitofwork.ServiceCenter.GetById(body.ServiceCenterID);
                    var lastID = unitofwork.Booking.getMaxId();
                    resp.data = unitofwork.Booking.GetById(lastID);
                    resp.error = 0;
                    resp.message = "Success";
                    var times = unitofwork.TimeSlots.GetById(body.FromID);
                    string TopPart = "<!DOCTYPE html><html lang='en' ><head><meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'>"
                                     + "  <meta name='viewport' content='width=device-width, initial-scale=1'><meta name='description' content=''><meta name='author' content=''><title>APSCO</title>"
                                     + "</head><body style='padding:0px;margin:0px;background-color:#f5f5f5;font-family: Helvetica !important;font-size: 14px !important;'>" +
                                      " <table style='background-color:#f5f5f5;width:100%;margin:0px;padding:0px;' cellpadding='0' cellspacing='0' border='0' width='100%' height='100%'>" +
                                      "<tr>" +
                                     "<td align='center' valign='top' style='width: 100%; height: 100%;'>" +
                                         "<table style='background-color:#fff;width:900px;margin:0 auto;padding:0px;box-shadow: 0px 0px 10px #C4C4CC;' cellpadding='0' cellspacing='0' border='0'>" +
                                             "<tr>" +
                                                 "<td style='height:20px;background-color: #0047BA;' colspan='2'></td>" +
                                             "</tr>" +
                                             "<tr>" +
                                                 "<td>" +
                                                     "<table width='100%' cellpadding='0' cellspacing='0'>" +
                                                         "<tr>" +
                                                             "<td style='padding-left:40px;background-color:#2B2926;height:120px;font-weight:bold;font-size:25px;color:white;'>" +
                                                                 "Mobil MSS Daily Report <br/></td>" +
                                     "<td style='background-color:#2B2926;width:160px;'>" +
                                         "<img src='http://mobilksa.com/Images/MobilBG.png' height='70' />" +
                                     "</td>" +
                                     "<td style='background-color:#2B2926;width:180px;padding:0px 10px;'>" +
                                        " <img src='http://mobilksa.com/Images/MobilBorder.png' height='50' />" +
                                     "</td>" +
                                 "</tr>" +
                             "</table></td></tr>";
                    string intro = "<div><p>Dear Site Manger</p><br/><p>Kindly find booking details as follow</p></div>";
                    string bodyhtml = "";
                    bodyhtml += "<tr><td align='center' style='background-color:#f1f1f1;'><table  width='100%' style='background-color:#e3e3e3;font-family:Helvetica;color:#85888B;'><tr><td style='padding:10px;text-align:center;font-weight:bold;'>Customer</td><td style='padding:10px;text-align:center;font-weight:bold;'>Mobile</td><td style='padding:10px;text-align:center;font-weight:bold;'>Vehicle</td><td style='padding:10px;text-align:center;font-weight:bold;'>Date</td><td style='padding:10px;text-align:center;font-weight:bold;'>Time from</td><td style='padding:10px;text-align:center;font-weight:bold;'>To</td><td style='padding:10px;text-align:center;font-weight:bold;'>Services</td><td style='padding:10px;text-align:center;font-weight:bold;'>Service center</td></tr>";
                    bodyhtml += "<tr style='background-color:white ;color: #2B2926;font-weight: bold;'><td style='padding:10px;text-align:center;'>" + customer.FirstName + " " + customer.LastName + "</td><td style='padding:10px;text-align:center;'>" + customer.Mobile + "</td><td style='padding:10px;text-align:center;'>" + BookingObj.Car_Code + "</td><td style='padding:10px;text-align:center;'>"
                        + BookingObj.BookingDate.Value.ToShortDateString() + "</td><td style='padding:10px;text-align:center;'>" + times.StartTime + "</td><td style='padding:10px;text-align:center;'>" + times.EndTime + "</td><td style='padding:10px;text-align:center;'>" + services.Remove(0, 1) + "</td><td style='padding:10px;text-align:center;'>" + ServiceCenter.CenterName + "</td></tr></table>  </td>"
                             + "   </tr>"
                            + "</table>"
                        + "</td>"
                    + "</tr>"
                    + "<tr>"
                     + "<td align='center' height='20px'>&nbsp;</td>"
                    + "</tr>"
                    + "<tr>"
                     + "<td style='background-color: #0047BA;height: 50px; color:#ffffff; text-align:center;font-size:14px;font-family:Helvetica;'>"
                      + "Copyrights 2017 © APSCO - Saudi Arabia, MSS (Developed by Select ICT)"
                       + "</td>"
                    + "</tr>"
                   + "</table>"
                   + "</body>"
                   + "</html>";
                    string ServiceCenterEmail = ServiceCenter.Email;
                    configuration.SendMail("Booking Details", intro + TopPart + bodyhtml, ServiceCenterEmail);
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    resp.error = 2;
                    resp.message = "Login again";
                    return Request.CreateResponse(HttpStatusCode.NotFound, resp);
                }
            }
            catch (Exception ex)
            {
                resp.error = 1;
                resp.message = "Check internet connection";
                return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
            }
        }

        [HttpPost]
        public HttpResponseMessage EditBooking([FromBody] EditBookingBody body)
        {

            MessageObject resp = new MessageObject();
            try
            {
                var BookingInDb = unitofwork.Booking.GetById(body.Id);
                if (BookingInDb == null)
                {
                    resp.error = 3;
                    resp.message = "Booking Not Exist";

                    return Request.CreateResponse(HttpStatusCode.NotFound, resp);
                }
                if (BookingInDb.FK_BookingStatusID != 1)
                {
                    resp.error = 4;
                    resp.message = "Cant Edit Booking Not Pending";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                }
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(body.token);
                if (checktoken == true)
                {
                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == body.token).Select(x => x.FK_CustomerID).FirstOrDefault();

                    //Check if times are available
                    DAL.Booking BookingObj = unitofwork.Booking.GetWhere(s => s.ID == body.Id).FirstOrDefault();
                    BookingObj.FK_CustomerID = CustomerID;
                    BookingObj.FK_ServiceCenterID = body.ServiceCenterID;
                    BookingObj.ID = body.Id;
                    if (body.CarCode == "-1")
                        BookingObj.Car_Code = null;
                    else
                        BookingObj.Car_Code = body.CarCode;

                    BookingObj.BookingDate = Convert.ToDateTime(body.ChoosenDate);
                    BookingObj.FK_BookingStatusID = 1;
                    BookingObj.LastModifiedDate = DateTime.Now;
                   // unitofwork.Booking.Update(BookingObj);
                    unitofwork.Commit();

                    DAL.BookingService BookingServ = new DAL.BookingService();
                    var BookingServiceinDb = unitofwork.BookingServices.GetWhere(b => b.FK_BookingID == body.Id).ToList();
                    foreach (var item in BookingServiceinDb)
                    {
                        unitofwork.BookingServices.Delete(item);
                    }
                    unitofwork.Commit();
                    string services = "";
                    foreach (var item in body.Services)
                    {
                        BookingServ.FK_ServiceID = item;
                        services += "," + (unitofwork.SalesItem.GetById(item).ItemName);
                        BookingServ.FK_BookingID = BookingObj.ID;
                        unitofwork.BookingServices.Insert(BookingServ);
                        unitofwork.Commit();
                    }

                    if (body.FromID != -1)
                    {
                        var TimeSlotsinDb = unitofwork.BookingTimeSlots.GetWhere(b => b.FK_BookingID == body.Id).ToList();
                        foreach (var item in TimeSlotsinDb)
                        {
                            unitofwork.BookingTimeSlots.Delete(item);

                        }
                        unitofwork.Commit();
                        DAL.BookingTimeSlot BookingTimes = new DAL.BookingTimeSlot();

                        //Insert  Times Slots
                        var SlotsList = unitofwork.TimeSlots.GetWhere(t => t.ID >= body.FromID && t.ID <= (body.FromID + (body.Inetrval))).ToList();

                        foreach (var item in SlotsList)
                        {
                            BookingTimes.FK_BookingID = BookingObj.ID;
                            BookingTimes.FK_TimeSlotID = item.ID;
                            unitofwork.BookingTimeSlots.Insert(BookingTimes);
                            unitofwork.Commit();
                        }

                    }
                    resp.error = 0;
                    resp.message = "Success";
                    var customer = unitofwork.customer.GetById(CustomerID);
                    var ServiceCenter = unitofwork.ServiceCenter.GetById(body.ServiceCenterID);

                    var times = unitofwork.TimeSlots.GetById(body.FromID);
                    var BookingTimeSlots = unitofwork.BookingTimeSlots.GetWhere(b => b.FK_BookingID == body.Id);
                    if (times == null)
                    {
                        times = unitofwork.TimeSlots.GetWhere(t => t.ID == BookingTimeSlots.FirstOrDefault().FK_TimeSlotID).FirstOrDefault();
                    }

                    string TopPart = "<!DOCTYPE html><html lang='en' ><head><meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'>"
                                     + "  <meta name='viewport' content='width=device-width, initial-scale=1'><meta name='description' content=''><meta name='author' content=''><title>APSCO</title>"
                                     + "</head><body style='padding:0px;margin:0px;background-color:#f5f5f5;font-family: Helvetica !important;font-size: 14px !important;'>" +
                                      " <table style='background-color:#f5f5f5;width:100%;margin:0px;padding:0px;' cellpadding='0' cellspacing='0' border='0' width='100%' height='100%'>" +
                                      "<tr>" +
                                     "<td align='center' valign='top' style='width: 100%; height: 100%;'>" +
                                         "<table style='background-color:#fff;width:900px;margin:0 auto;padding:0px;box-shadow: 0px 0px 10px #C4C4CC;' cellpadding='0' cellspacing='0' border='0'>" +
                                             "<tr>" +
                                                 "<td style='height:20px;background-color: #0047BA;' colspan='2'></td>" +
                                             "</tr>" +
                                             "<tr>" +
                                                 "<td>" +
                                                     "<table width='100%' cellpadding='0' cellspacing='0'>" +
                                                         "<tr>" +
                                                             "<td style='padding-left:40px;background-color:#2B2926;height:120px;font-weight:bold;font-size:25px;color:white;'>" +
                                                                 "Mobil MSS Daily Report <br/></td>" +
                                     "<td style='background-color:#2B2926;width:160px;'>" +
                                         "<img src='http://mobilksa.com/Images/MobilBG.png' height='70' />" +
                                     "</td>" +
                                     "<td style='background-color:#2B2926;width:180px;padding:0px 10px;'>" +
                                        " <img src='http://mobilksa.com/Images/MobilBorder.png' height='50' />" +
                                     "</td>" +
                                 "</tr>" +
                             "</table></td></tr>";
                    string intro = "<div><p>Dear Site Manger</p><br/><p>Kindly find booking details as follow</p></div>";
                    string bodyhtml = "";
                    bodyhtml += "<tr><td align='center' style='background-color:#f1f1f1;'><table  width='100%' style='background-color:#e3e3e3;font-family:Helvetica;color:#85888B;'><tr><td style='padding:10px;text-align:center;font-weight:bold;'>Customer</td><td style='padding:10px;text-align:center;font-weight:bold;'>Mobile</td><td style='padding:10px;text-align:center;font-weight:bold;'>Vehicle</td><td style='padding:10px;text-align:center;font-weight:bold;'>Date</td><td style='padding:10px;text-align:center;font-weight:bold;'>Time from</td><td style='padding:10px;text-align:center;font-weight:bold;'>To</td><td style='padding:10px;text-align:center;font-weight:bold;'>Services</td><td style='padding:10px;text-align:center;font-weight:bold;'>Service center</td></tr>";
                    bodyhtml += "<tr style='background-color:white ;color: #2B2926;font-weight: bold;'><td style='padding:10px;text-align:center;'>" + customer.FirstName + " " + customer.LastName + "</td><td style='padding:10px;text-align:center;'>" + customer.Mobile + "</td><td style='padding:10px;text-align:center;'>" + BookingObj.Car_Code + "</td><td style='padding:10px;text-align:center;'>"
                        + BookingObj.BookingDate.Value.ToShortDateString() + "</td><td style='padding:10px;text-align:center;'>" + times.StartTime + "</td><td style='padding:10px;text-align:center;'>" + times.EndTime + "</td><td style='padding:10px;text-align:center;'>" + services.Remove(0, 1) + "</td><td style='padding:10px;text-align:center;'>" + ServiceCenter.CenterName + "</td></tr></table>  </td>"
                             + "   </tr>"
                            + "</table>"
                        + "</td>"
                    + "</tr>"
                    + "<tr>"
                     + "<td align='center' height='20px'>&nbsp;</td>"
                    + "</tr>"
                    + "<tr>"
                     + "<td style='background-color: #0047BA;height: 50px; color:#ffffff; text-align:center;font-size:14px;font-family:Helvetica;'>"
                      + "Copyrights 2017 © APSCO - Saudi Arabia, MSS (Developed by Select ICT)"
                       + "</td>"
                    + "</tr>"
                   + "</table>"
                   + "</body>"
                   + "</html>";
                    string ServiceCenterEmail = ServiceCenter.Email;
                    configuration.SendMail("Booking Details", intro + TopPart + bodyhtml, ServiceCenterEmail);
                    return Request.CreateResponse(HttpStatusCode.Created, resp);
                }
                else
                {
                    resp.error = 2;
                    resp.message = "Login again";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                }
            }
            catch (Exception ex)
            {
                resp.error = 1;
                resp.message = "Check internet connection";
                return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
            }
        }
        [HttpPost]
        public HttpResponseMessage CancelBooking([FromBody] CancelBookingBody body)
        {
            MessageObject resp = new MessageObject();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(body.token);
                if (checktoken == true)
                {

                    resp.error = 0;
                    resp.message = "Success";

                    var BookingObj = unitofwork.Booking.GetById(body.BookingID);

                    BookingObj.FK_BookingStatusID = 3;
                    BookingObj.FK_CancelReasonID = body.ReasonID;
                    BookingObj.IsUserCancel = true;
                    BookingObj.LastModifiedDate = DateTime.Now;

                    unitofwork.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    resp.error = 2;
                    resp.message = "Login again";
                    return Request.CreateResponse(HttpStatusCode.NotFound, resp);
                }
            }
            catch (Exception ex)
            {
                resp.error = 1;
                resp.message = "Check internet connection";
                //   resp.message = ex.Message+" "+ex.InnerException;
                return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
            }
        }

        [HttpGet]
        public CancelReasonsResponse CancelReasons(string lang)
        {
            CancelReasonsResponse resp = new CancelReasonsResponse();
            try
            {
                resp.data = new List<CancelReasonsData>();
                var ResounsList = unitofwork.BookingCancelReasons.GetWhere(x => x.IsMobile == true || x.IsMobilePortal == true).ToList();
                foreach (var item in ResounsList)
                {
                    CancelReasonsData CancelReasonObj = new CancelReasonsData();
                    CancelReasonObj.ID = item.ID;
                    if (lang == "ar")
                        CancelReasonObj.Name = item.NameAr;
                    else
                        CancelReasonObj.Name = item.Name;
                    resp.data.Add(CancelReasonObj);
                }
                resp.error = 0;
                resp.message = "Success";
            }
            catch (Exception ex)
            {
                resp.error = 1;
                resp.message = "Check internet connection";
            }
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage GetBookingStatus()
        {
            LookupResponse resp = new LookupResponse();
            try
            {
                resp.error = 0;
                resp.message = "Success";
                resp.Data = new List<BookingStatusData>();
                var BookingStatusList = unitofwork.BookingStatus.GetAll();
                foreach (var item in BookingStatusList)
                {
                    BookingStatusData BookingStatusObj = new BookingStatusData();
                    BookingStatusObj.ID = item.ID;
                    BookingStatusObj.Name = item.Name;
                    resp.Data.Add(BookingStatusObj);

                }
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                resp.error = 1;
                resp.message = "Check internet connection";
                return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
            }
        }
        // PUT api/booking/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/booking/5
        public void Delete(int id)
        {
        }
        public static void SendPushNotification(string NotifyBody, string NotifyTitle, string deviceId)
        {
            try
            {
                var result = "-1";
                string applicationID = "AIzaSyA9cLIu1kVaB11SXUefOHk6szdOiSSnIdE";
                string senderId = "46219237439";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";

                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                using (var streamWriter = new StreamWriter(tRequest.GetRequestStream()))
                {
                    string json = "{\"to\": \"" + deviceId +
                       "\",\"data\": {\"body\":{\"message\":\"" + NotifyBody + "\",\"title\":\"" + NotifyTitle + "\",\"flag\":\"" + NotifyTitle +
                       "\"}},\"notification\":{\"title\":\"" + NotifyTitle + "\",\"text\":\"" + NotifyBody + "\",\"sound\":\"default\",\"content_available\":\"true\"}}";
                    //string json1 = "{\"to\": \"" + deviceId +
                    //   "\",\"data\": {\"body\":{\"title\":\"" + NotifyTitle + "\",\"message\":\"" + NotifyBody +
                    //   "\"}}}";
                    //string json = "{\"to\": \"" + deviceId +
                    //   "\",\"data\": {\"body\": \"" + NotifyBody + "\",\"title\":\"" + NotifyTitle + "\",\"flag\":\"" + NotifyTitle +
                    //   "\"}";
                    //  string json = "\"message\":{\"token\":" + deviceId + ",\"data\":{\"body\":"+NotifyBody+",\"title\":"+NotifyTitle+"} } ";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)tRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
    }
}
