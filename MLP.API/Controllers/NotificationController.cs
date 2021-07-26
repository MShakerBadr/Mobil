using MLP.BAL;
using MLP.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MLP.API.Controllers
{
    public class NotificationController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();
        // GET api/notification
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/notification/5
        public NotificationResponse Get(string token)
        {
            NotificationResponse Notify = new NotificationResponse();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);
                if (checktoken == true)
                {
                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();

                    Notify.error = 0;
                    Notify.message = "Success";
                    Notify.data = new List<NotificationsData>();

                    var CustomerNotifications = unitofwork.CustomerNotification.GetWhere(x => x.FK_CustomerID == CustomerID).ToList();
                    var ClearDate = unitofwork.NotfifcationClearDate.GetAll().Where(s => s.FK_CustomerID == CustomerID).OrderByDescending(s=>s.ID).FirstOrDefault();
                    var Notifications = unitofwork.Notifications.GetAll().Where(s => s.IsActive == true&&s.FK_NotificationTypeID!=6).ToList();
                    if (ClearDate!=null)
                    {
                        CustomerNotifications= unitofwork.CustomerNotification.GetWhere(x => x.FK_CustomerID == CustomerID && x.MLPNotification.NotificationDate > ClearDate.Cleardate).ToList();
                        Notifications = unitofwork.Notifications.GetAll().Where(s => s.IsActive == true&&s.NotificationDate > ClearDate.Cleardate).ToList();
                    }
                    //var Notifications = unitofwork.Notifications.GetAll().Where(s => s.IsActive == true).ToList();
                    CustomerNotifications.ForEach(CustNotify =>
                    {
                        Notify.data.Add(new NotificationsData
                        {
                            Id = CustNotify.MLPNotification.ID,
                            Message = CustNotify.MLPNotification.NotificationMessage,
                            Date = CustNotify.MLPNotification.NotificationDate.Value.ToString("dd/MM/yyyy").Replace("-", "/"),
                            NotificationType = CustNotify.MLPNotification.MLPNotificationType.Type,
                            IsSeen = CustNotify.IsSeen
                        });
                    });
                    Notifications.ForEach(CustNotify =>
                    {
                        if (Notify.data.Select(s=>s.Id).Contains(CustNotify.ID))
                        {

                        }else
                        {
                            Notify.data.Add(new NotificationsData
                            {
                                Id = CustNotify.ID,
                                Message = CustNotify.NotificationMessage,
                                Date = CustNotify.NotificationDate.Value.ToString("dd/MM/yyyy").Replace("-", "/"),
                                NotificationType = CustNotify.MLPNotificationType.Type,
                                DateTimeNotification = CustNotify.NotificationDate.Value.ToString(),
                                IsSeen = false
                            });
                        }
                       
                    });
                  //  Notify.data=Notify.data.Distinct().GroupBy(s=>s.Id).Select(s=>new NotificationsData {Id=s.Key }).ToList();

                }
                else
                {
                    Notify.error = 2;
                    Notify.message = "Login again";
                }
            }
            catch (Exception ex)
            {
                Notify.error = 1;
                Notify.message = "Check internet connection";
            }
            return Notify;
        }
        [HttpGet]
        public NotificationResponse ClearNotification(string token)
        {
            NotificationResponse Notify = new NotificationResponse();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);
                if (checktoken == true)
                {
                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();

                  
                    var ClearDate = unitofwork.NotfifcationClearDate.GetAll().Where(s => s.FK_CustomerID == CustomerID).FirstOrDefault();
                   
                    if (ClearDate != null)
                    {
                        ClearDate.Cleardate = DateTime.Now;
                        unitofwork.Commit();
                    }
                    else
                    {
                        unitofwork.NotfifcationClearDate.Insert(new DAL.CustomersClearNotification { Cleardate = DateTime.Now, CreationDate = DateTime.Now, FK_CustomerID = CustomerID });
                        unitofwork.Commit();
                    }
                    Notify.error = 0;
                    Notify.message = "Success";
                    Notify.data = new List<NotificationsData>();
                }
                else
                {
                    Notify.error = 2;
                    Notify.message = "Login again";
                }
            }
            catch (Exception ex)
            {
                Notify.error = 1;
                Notify.message = "Check internet connection";
            }
            return Notify;
        }

        // POST api/notification
        public void Post([FromBody]string value)
        {
        }

        // PUT api/notification/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/notification/5
        public void Delete(int id)
        {
        }
    }
}
