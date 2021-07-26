using MLP.API.Utilities;
using MLP.BAL;
using MLP.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MLP.API.Controllers
{
    public class MobilContactController : ApiController
    {
        // GET api/mobilcontact
        UnitOfWork unitofwork = new UnitOfWork();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/mobilcontact/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/mobilcontact
        public MessageObject Post([FromBody]MobilContact body)
        {
            MessageObject resp = new MessageObject();
            try
            {

                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(body.token);

                if (checktoken == true)
                {
                    var CustomerObj = unitofwork.LoginLog.GetWhere(x => x.Token == body.token).Select(x => x.Customer).FirstOrDefault();
                    //var CustomerObj = unitofwork.customer.GetById(CustomerID);
                    resp.error = 0;
                    resp.message = "Success";
                    string Body = "";
                    string MessageDate = "";
                    var ThreadObj = unitofwork.MessageThread.GetWhere(x => x.ThreadTitle == body.MessageTitle).FirstOrDefault();
                    if (ThreadObj != null)
                    {
                        DAL.MessageComment MsgDetails = new DAL.MessageComment();
                        MsgDetails.ContactMessage = body.MessageBody;
                        MsgDetails.FK_MessageThreadID = ThreadObj.ID;
                        MsgDetails.MessageDate = DateTime.Now;
                        // MsgDetails.FK_SenderID = 1;
                        MsgDetails.IsActive = true;
                        MsgDetails.MessageType = "Message";
                        MsgDetails.LastModifiedDate = DateTime.Now;
                        unitofwork.MessageComment.Insert(MsgDetails);
                        unitofwork.Commit();

                        MessageDate = MsgDetails.MessageDate.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        DAL.MessageThread Msg = new DAL.MessageThread();
                        Msg.ThreadTitle = body.MessageTitle;
                        Msg.FK_CustomerID = CustomerObj.ID;
                        Msg.CreationDate = DateTime.Now;
                        Msg.LastModifiedDate = DateTime.Now;
                        Msg.IsActive = true;
                        unitofwork.MessageThread.Insert(Msg);
                        unitofwork.Commit();

                        DAL.MessageComment MsgDetails = new DAL.MessageComment();
                        MsgDetails.ContactMessage = body.MessageBody;
                        MsgDetails.FK_MessageThreadID = Msg.ID;
                        MsgDetails.MessageDate = DateTime.Now;
                        // MsgDetails.FK_SenderID = 1;
                        MsgDetails.IsActive = true;
                        MsgDetails.MessageType = "Message";
                        MsgDetails.LastModifiedDate = DateTime.Now;
                        unitofwork.MessageComment.Insert(MsgDetails);
                        unitofwork.Commit();

                        MessageDate = MsgDetails.MessageDate.Value.ToString("dd/MM/yyyy");
                    }


                    #region send mail to Mobil

                    string ToMail = ConfigurationManager.AppSettings["ContactMobilMail"].ToString();

                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/ContactMobil.html")))
                    {
                        Body = reader.ReadToEnd();
                    }

                    Body = Body.Replace("{Date}", MessageDate);
                    Body = Body.Replace("{CustomerName}", CustomerObj.FirstName + " " + CustomerObj.LastName);
                    Body = Body.Replace("{CustomerMobile}", CustomerObj.Mobile);
                    Body = Body.Replace("{Message}", body.MessageBody);

                    #endregion


                    configuration.SendMail("Mobil Service Contact Request - " + body.MessageTitle, Body, ToMail);

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

        // PUT api/mobilcontact/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/mobilcontact/5
        public void Delete(int id)
        {
        }
    }
}
