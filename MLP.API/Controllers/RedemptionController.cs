using MLP.API.Utilities;
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
    public class RedemptionController : ApiController
    {
        // GET api/redemption
        UnitOfWork unitofwork = new UnitOfWork();
        configuration conf = new configuration();
        public RedemptionListResponse Get(string token, string lang)
        {
            RedemptionListResponse resp = new RedemptionListResponse();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);
                if (checktoken == true)
                {
                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    resp.error = 0;
                    resp.message = "Success";
                    resp.Data = new List<RedeemData>();
                    var RedeemList = unitofwork.RedemptionRequests.GetWhere(x => x.FK_CustomerID == CustomerID && x.FK_RedeemStatusID == 5).OrderByDescending(x => x.RedeemDate).ToList();
                    foreach (var item in RedeemList)
                    {
                        RedeemData RedeemItem = new RedeemData();

                        RedeemItem.ID = item.FK_AwardID.Value;
                        RedeemItem.Award = unitofwork.SalesItem.GetWhere(x => x.ID == item.Award.FK_SalesItemID).Select(x => lang == "ar" ? x.ItemNameAr : x.ItemName).FirstOrDefault() ?? string.Empty;
                        RedeemItem.Image = item.Award.AwardImage ?? string.Empty;
                        RedeemItem.ServiceCenter = item.FK_ServiceCenterID == null ? "" : (lang == "ar" ? item.ServiceCenter.CenterNameAr : item.ServiceCenter.CenterName) ?? string.Empty;
                        RedeemItem.RedeemPoints = item.RedeemPoints ?? 0;
                        RedeemItem.RedeemDate = item.RedeemDate.ToString();

                        resp.Data.Add(RedeemItem);
                    }
                }
                else
                {
                    resp.error = 2;
                    resp.message = "Login again";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return resp;
        }

        // GET api/redemption/5
        [HttpGet]
        public RedeemReqResponse RedeemRequest(string token, int AwardID)
        {
            RedeemReqResponse resp = new RedeemReqResponse();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);

                if (checktoken == true)
                {
                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    var CustomerObj = unitofwork.customer.GetById(CustomerID);
                    var AwardObj = unitofwork.Awards.GetWhere(a => a.IsActive == true && a.ID == AwardID).FirstOrDefault();
                    if ((//CustomerObj.FK_LatestLevelID >= AwardObj.FK_LevelID && 
                        
                        CustomerObj.CurrentPoints >= AwardObj.AwardDuePoints))
                    {
                        string Vcode = conf.GenerateNumericVerficationCode();
                        string POSCode = conf.GenerateNumericVerficationCode();
                        DAL.RedemptionRequest Redem = new DAL.RedemptionRequest();
                        Redem.FK_CustomerID = CustomerID;
                        Redem.FK_AwardID = AwardID;
                        Redem.RedeemDate = DateTime.Now;
                        Redem.LastModifiedDate = DateTime.Now;
                        Redem.CustomerConfirmCode = Vcode;
                        Redem.RedemptionConfirmCode = POSCode;
                        Redem.FK_RedeemStatusID = 1;
                        unitofwork.RedemptionRequests.Insert(Redem);
                        unitofwork.Commit();
                        resp.error = 0;
                        resp.message = "Success";
                        resp.Code = Vcode;
                    }
                    else
                    {
                        resp.error = 3;
                        resp.message = "you are not allowed to redeem";
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

        [HttpGet]
        public MessageObject RedeemCancel(string token, string Code)
        {
            MessageObject resp = new MessageObject();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);

                if (checktoken == true)
                {

                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    var CustomerObj = unitofwork.customer.GetById(CustomerID);
                    var RedeemObj = unitofwork.RedemptionRequests.GetWhere(x => x.CustomerConfirmCode == Code).FirstOrDefault();
                    if (RedeemObj != null)
                    {
                        RedeemObj.FK_RedeemStatusID = 3;
                        unitofwork.Commit();
                    }
                    resp.error = 0;
                    resp.message = "Success";
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

        [HttpGet]
        public RedeemConfirmResponse RedeemConfirm(string token, string lang, string CustomerCode, string POSCode)
        {

            RedeemConfirmResponse resp = new RedeemConfirmResponse();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);

                if (checktoken == true)
                {
                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    var CustomerObj = unitofwork.customer.GetById(CustomerID);
                    var RedeemObj = unitofwork.RedemptionRequests.GetWhere(x => x.RedemptionConfirmCode == POSCode && x.CustomerConfirmCode == CustomerCode && !string.IsNullOrEmpty(x.RedemptionConfirmCode)).FirstOrDefault();

                    if (RedeemObj != null)
                    {
                        //subtract from Customer
                        int Points = (CustomerObj.CurrentPoints - RedeemObj.Award.AwardDuePoints) ?? 0;
                        // CustomerObj.CurrentPoints = Points;
                        RedeemObj.FK_RedeemStatusID = 4; // Status of Success
                        RedeemObj.RedeemPoints = RedeemObj.Award.AwardDuePoints;
                        //Subtract from Inventory

                        unitofwork.Commit();
                        //Return Customer Data
                        resp.CurrentPoints = Points;
                        resp.error = 0;
                        resp.message = "Success";
                        resp.Data = new List<RedeemData>();
                        var RedeemReqObj = unitofwork.RedemptionRequests.GetWhere(x => x.FK_CustomerID == CustomerID && x.CustomerConfirmCode == CustomerCode && x.RedemptionConfirmCode == POSCode).FirstOrDefault();
                        //foreach (var item in RedeemList)
                        //{
                        RedeemData RedeemItem = new RedeemData();

                        RedeemItem.ID = RedeemReqObj.FK_AwardID.Value;
                        RedeemItem.Award = unitofwork.SalesItem.GetWhere(x => x.ID == RedeemReqObj.Award.FK_SalesItemID).Select(x => lang == "ar" ? x.ItemNameAr : x.ItemName).FirstOrDefault() ?? string.Empty;
                        RedeemItem.Image = RedeemReqObj.Award.AwardImage ?? string.Empty;
                        RedeemItem.ServiceCenter = RedeemReqObj.FK_ServiceCenterID == null ? "" : (lang == "ar" ? RedeemReqObj.ServiceCenter.CenterNameAr : RedeemReqObj.ServiceCenter.CenterName) ?? string.Empty;
                        RedeemItem.RedeemPoints = RedeemReqObj.RedeemPoints ?? 0;
                        RedeemItem.RedeemDate = RedeemReqObj.RedeemDate.ToString();

                        resp.Data.Add(RedeemItem);
                        //}
                    }
                    else
                    {
                        resp.error = 3;
                        resp.message = "Invalid Code";
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
                var RedeemObj = unitofwork.RedemptionRequests.GetWhere(x => x.RedemptionConfirmCode == POSCode && x.CustomerConfirmCode == CustomerCode).FirstOrDefault();
                if (RedeemObj != null)
                {
                    RedeemObj.FK_RedeemStatusID = 6; // Status of Fail
                    unitofwork.Commit();
                }
                resp.error = 1;
                resp.message = "Check internet connection";
            }
            return resp;
        }


        // POST api/redemption
        [HttpPost]
        public void Post([FromBody]string body)
        {

        }

        // PUT api/redemption/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/redemption/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        public RedeemReqResponse AlForsanRedeemRequest([FromBody] AlForsanViewModel body)
        {
            RedeemReqResponse resp = new RedeemReqResponse();
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(body.token);
                if (checktoken == true)
                {
                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == body.token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    var CustomerObj = unitofwork.customer.GetById(CustomerID);
                    var AwardObj = unitofwork.Awards.GetWhere(a => a.IsActive == true && a.ID == body.AwardID).FirstOrDefault();
                    if (CustomerObj.CurrentPoints >= AwardObj.AwardDuePoints)
                    {
                        //Inser reedeem request
                        DAL.RedemptionRequest Redem = new DAL.RedemptionRequest();
                        //Redem.FK_CustomerID = CustomerID;
                        Redem.FK_AwardID = body.AwardID;
                        Redem.RedeemDate = DateTime.Now;
                        Redem.RedeemPoints = body.Points;
                        Redem.FK_RedeemStatusID = 4;
                        Redem.LastModifiedDate = DateTime.Now;
                        unitofwork.RedemptionRequests.Insert(Redem);
                        unitofwork.Commit();

                        //Get last inserted redeem
                        //int LastReedeemRequest= unitofwork.RedemptionRequests.GetAll().OrderByDescending(x => x.ID).FirstOrDefault().ID;

                        //Insert forsan redeem 
                        DAL.AlForsanRedemption AlForsanRedemption = new DAL.AlForsanRedemption();
                        AlForsanRedemption.FirstName = body.FirstName;
                        AlForsanRedemption.LastName = body.LastName;
                        AlForsanRedemption.MembershipNumber = body.MembershipNo;
                        AlForsanRedemption.RedeemMiles = body.Miles;
                        AlForsanRedemption.FK_RedemptionRequest = Redem.ID;
                        //Statues
                        //LAst
                        unitofwork.AlForsanRedemption.Insert(AlForsanRedemption);
                        unitofwork.Commit();

                        //update points for customer && fursan MembershipNo
                        CustomerObj.CurrentPoints -= body.Points;
                        CustomerObj.AlfursanNo = body.MembershipNo;
                        CustomerObj.LastModifiedDate = DateTime.Now;
                        unitofwork.customer.Update(CustomerObj);
                        unitofwork.Commit();

                        resp.error = 0;
                        resp.message = "Success";
                        resp.CurrentPoints = (int)CustomerObj.CurrentPoints;
                        resp.AlfursanNo = CustomerObj.AlfursanNo ?? string.Empty;
                    }
                    else
                    {
                        resp.error = 3;
                        resp.message = "you are not allowed to redeem";
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
    }
}
