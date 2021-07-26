using MLP.BAL;
using MLP.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace MLP.API.Controllers
{
    public class DeviceTokenController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();
        // GET api/DeviceToken
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/DeviceToken/5
        [ValidateInput(false)]

        public NotificationResponse Get(string UserToken, string FirBaseToken)
        {
            NotificationResponse Notify = new NotificationResponse();
            try
            {
                var CurrentData = unitofwork.MLPTokenlst.GetWhere(s => s.UserToken == UserToken).FirstOrDefault();
                if (CurrentData != null)
                {
                    CurrentData.FireBaseToken = FirBaseToken;
                }
                else
                {
                    unitofwork.MLPTokenlst.Insert(new DAL.MLPCustomerToken() { UserToken = UserToken, FireBaseToken = FirBaseToken });
                }
                unitofwork.Commit();
                Notify.error = 0;
                Notify.message = "Success";
            }
            catch (Exception ex)
            {
                Notify.error = 1;
                Notify.message = "Check internet connection";
            }
            return Notify;
        }

        // POST api/DeviceToken
        public NotificationResponse Post([FromBody]FirBaseVM fireBase)
        {
            NotificationResponse Notify = new NotificationResponse();
            try
            {
                var CurrentData = unitofwork.MLPTokenlst.GetWhere(s => s.UserToken == fireBase.UserToken).FirstOrDefault();
                if (CurrentData != null)
                {
                    CurrentData.FireBaseToken = fireBase.FirBaseToken;
                }
                else
                {
                    unitofwork.MLPTokenlst.Insert(new DAL.MLPCustomerToken() { UserToken = fireBase.UserToken, FireBaseToken = fireBase.FirBaseToken });
                }
                unitofwork.Commit();
                Notify.error = 0;
                Notify.message = "Success";
            }
            catch (Exception ex)
            {
                Notify.error = 1;
                Notify.message = "Check internet connection";
            }
            return Notify;
        }
        public class FirBaseVM
        {
            public string UserToken { get; set; }
            public string FirBaseToken { get; set; }
        }
        // PUT api/DeviceToken/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/DeviceToken/5
        public void Delete(int id)
        {
        }
    }
}
