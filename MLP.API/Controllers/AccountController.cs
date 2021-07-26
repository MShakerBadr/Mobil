using MLP.API.Utilities;
using MLP.BAL;
using MLP.BAL.ViewModels;
using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace MLP.API.Controllers
{
    public class AccountController : ApiController
    {

        UnitOfWork unitofwork = new UnitOfWork();
        configuration conf = new configuration();

        [HttpGet]
        public LoginResponse Login(string mobile, string password)
        {
            LoginResponse resp = new LoginResponse();
            try
            {
                password = conf.GenerateToken(password);
                DateTime Today = Convert.ToDateTime(DateTime.Now.ToShortDateString());
               
                string token = "";

                var Customer = unitofwork.customer.Checklogincredentials(mobile, password);
                if (Customer != null)
                {
                    if (Customer.IsFristlogin==true)
                    {
                        var startdate = DateTime.Parse(ConfigurationManager.AppSettings["OfferStartTime"]);
                        var enddate = DateTime.Parse(ConfigurationManager.AppSettings["OfferEndTime"]);
                        if (startdate <= DateTime.Now && enddate > DateTime.Now)
                        {
                            //var Cust = unitofwork.customer.GetWhere(s => s.Mobile == Customer.Mobile).FirstOrDefault();
                            //if (Cust.IsFristlogin == true)
                            //{
                            var Data = unitofwork.MLPSMSCheck.GetAll().FirstOrDefault(s => s.CustomerMobile == Customer.Mobile);
                            if (Data == null)
                            {
                                MLPSMSConfirmation NewObj = new MLPSMSConfirmation();
                                NewObj.CreationDate = DateTime.Now;
                                NewObj.IsActive = true;
                                NewObj.LastModificationDate = DateTime.Now;
                                NewObj.CustomerMobile = Customer.Mobile;
                                NewObj.FirstLoginMS = false;
                                NewObj.MobilOilMS = false;
                                NewObj.UpLevelMS = false;
                                unitofwork.MLPSMSCheck.Insert(NewObj);
                                unitofwork.Commit();
                            }
                            Data = unitofwork.MLPSMSCheck.GetAll().FirstOrDefault(s => s.CustomerMobile == Customer.Mobile);

                            if (Data.FirstLoginMS != true)
                            {
                                conf.SendSMSNew(ConfigurationManager.AppSettings["FirstLoginMsg"].ToString(), Customer.Mobile);
                                Data.LastModificationDate = DateTime.Now;
                                Data.FirstLoginMS = true;
                                unitofwork.Commit();
                            }

                            // }
                        }
                    }
                    var CheckFirstLogin = unitofwork.LoginLog.GetWhere(l => l.FK_CustomerID == Customer.ID && l.LogDate == Today).FirstOrDefault();
                    if (CheckFirstLogin == null)
                    {
                        token = conf.GenerateToken(Customer.ID.ToString());
                        LoginLog Log = new LoginLog();
                        Log.FK_CustomerID = Customer.ID;
                        Log.LogDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        Log.Token = token;
                       
                        unitofwork.LoginLog.Insert(Log);
                        unitofwork.Commit();
                        
                    }
                    else
                    {
                        token = CheckFirstLogin.Token;
                        LoginLog Log = new LoginLog();
                        Log.FK_CustomerID = Customer.ID;
                        Log.LogDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        Log.Token = token;
                        unitofwork.LoginLog.Insert(Log);
                        unitofwork.Commit();

                    }
                  
                    var CustomerFristLogin = unitofwork.LoginLog.GetWhere(l=>l.FK_CustomerID== Customer.ID);
                    if (CustomerFristLogin == null)
                    {
                        Customer.IsFristlogin = true;
                    }
                    else
                    {
                       
                      
                        Customer.IsFristlogin = false;
                    }
                   
                    resp = unitofwork.customer.GetCustomizeUserData(mobile, password);
                    Customer.IsFristlogin = false;
                    unitofwork.Commit();
                    resp.error = 0;
                    resp.message = "Success";
                    resp.LoginDate = DateTime.Now;
                    resp.Token = token;
                }
                else
                {
                    resp = new LoginResponse { error = 2, message = "Invalid Mobile number or password" };

                }
                return resp;

            }
            catch (Exception ex)
            {

                resp = new LoginResponse { error = 1, message = "Check internet connection" };

            }
            return resp;

        }
        public bool CheckIFCustomerBuyMobilOil(string Mobile)
        {
            var LstIDItems = unitofwork.InvoiceDetails.GetWhere(s => s.POSInvoice.FK_CustomerCode == Mobile).Select(S => S.FK_SalesItemID).ToList();
            var MobilItems = unitofwork.SalesItem.GetWhere(s => LstIDItems.Contains(s.ID) && s.IsMobileProduct).Count();
            if (MobilItems > 0)
            {
                return true;
            }
            return false;
        }
        [HttpGet]
        public CheckMobileResponse CheckmobileNO(string mobile, string password)
        {
            CheckMobileResponse resp;
            try
            {
                int x = unitofwork.customer.CheckRegistrationbymobil(mobile, password);
                if (x == 1)
                {
                    resp = new CheckMobileResponse { error = 3, message = "not exist,Register" };
                }
                else if (x == 2)
                {

                    password = conf.GenerateToken(password);
                    string Vcode = conf.GenerateVerficationCode();
                    unitofwork.customer.UpdateConfirmationCode(mobile, Vcode, password);
                    conf.SendSMSNew(Vcode + " is your Mobil Serivce App verification code. Please use it to verify your mobile.", mobile);
                    resp = new CheckMobileResponse { error = 4, message = "Exist but not mobil user" };
                }
                else
                {
                    resp = new CheckMobileResponse { error = 5, message = "User already exist,please login" };
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "unsupported destination")
                    resp = new CheckMobileResponse { error = 6, message = "Invalid Mobile No" };
                else
                    resp = new CheckMobileResponse { error = 1, message = "Check internet connection" };

            }
            return resp;
        }

        [HttpPost]
        public MessageObject Register([FromBody]RegistrtionParams Params)
        {
            MessageObject resp;
            try
            {
                string Vcode = conf.GenerateVerficationCode();
                string password = conf.GenerateToken(Params.password);
                var AreaObj = unitofwork.Areas.GetById(Params.Area);
                TempCustomer temp = new TempCustomer();
                temp.FirstName = Params.fname;
                temp.LastName = Params.lname;
                temp.Mobile = Params.Mobile;
                temp.Area = AreaObj.AreaNameEN;
                temp.Email = Params.Email;
                temp.Password = password;
                temp.ConfirmationCode = Vcode;
                temp.CreationDate = DateTime.Now;
                temp.LastModifiedDate = DateTime.Now;
                temp.FK_CityID = Params.city;
                temp.Gender = Params.gender;
                temp.BirthDate = Convert.ToDateTime(Params.dateofbrith);
                temp.FK_AreaID = Params.Area;
                unitofwork.Tempcustomer.Insert(temp);
                unitofwork.Commit();

                conf.SendSMSNew(Vcode + " is your Mobil Serivce App verification code. Please use it to verify your mobile.", Params.Mobile);

                resp = new MessageObject();
                resp.error = 0;
                resp.message = "Success";
            }
            catch (Exception ex)
            {
                if (ex.Message == "unsupported destination")
                    resp = new MessageObject { error = 2, message = "Invalid Mobile No" };
                else
                    resp = new MessageObject { error = 1, message = "Server Error " };

            }
            return resp;
        }

        [HttpPost]
        public MessageObject ConfirmUserCode([FromBody]VerficationParams Params)
        {
            MessageObject resp;
            try
            {
                int x = unitofwork.customer.ValidateConfirmationCode(Params.mobile, Params.code);
                int y = unitofwork.Tempcustomer.ValidateConfirmationCode(Params.mobile, Params.code);

                if (x == 0 && y == 0)
                {
                    resp = new MessageObject { error = 6, message = "Invalid Confirmation Code" };
                }
                else
                {
                    InsertLoginLog(Params.mobile);
                    resp = new MessageObject { error = 0, message = "Success" };
                }

            }
            catch (Exception ex)
            {
                resp = new MessageObject { error = 1, message = "Check internet connection" };

            }
            return resp;
        }

        [HttpPost]
        public CustomerResponse EditCustomer([FromBody]RegistrtionParams Params, string token)
        {
            CustomerResponse CustObj = new CustomerResponse();
            try
            {
                bool check = unitofwork.LoginLog.CheckTokenValidty(token);
                if (check == true)
                {
                    var AreaObj = unitofwork.Areas.GetById(Params.Area);
                    int CustomerID = unitofwork.LoginLog.GetWhere(l => l.Token == token).Select(l => l.FK_CustomerID).FirstOrDefault();
                    Customer CustomerObj = unitofwork.customer.GetById(CustomerID);
                    CustomerObj.FirstName = Params.fname;
                    CustomerObj.LastName = Params.lname;
                    //  CustomerObj.Mobile = Params.Mobile;
                    CustomerObj.Email = Params.Email;
                    CustomerObj.Area = AreaObj.AreaNameEN;
                    CustomerObj.FK_AreaID=Params.Area;
                    CustomerObj.FK_CityID = Params.city;
                    CustomerObj.Gender = Params.gender;
                    CustomerObj.BirthDate = Convert.ToDateTime(Params.dateofbrith);
                    CustomerObj.AlfursanNo = Params.AlfursanNo == "" ? null
                                           : Params.AlfursanNo;

                    unitofwork.customer.Update(CustomerObj);
                    unitofwork.Commit();

                    CustObj.data = unitofwork.customer.GetCustomizeUserData(CustomerID);
                    CustObj.error = 0;
                    CustObj.message = "Success";
                }
                else
                {
                    CustObj.error = 2;
                    CustObj.message = "Login again";
                }
            }
            catch (Exception)
            {
                CustObj.error = 1;
                CustObj.message = "Check internet connection";
            }
            return CustObj;
        }

        [HttpGet]
        public MessageObject ForgetPassword(string mobile)
        {
            MessageObject resp;
            try
            {
                // mobile send code 05xxxxx
                string modifiedMobile = mobile.Substring(1, (mobile.Length - 1));
                var CustomerObj = unitofwork.customer.GetWhere(c => c.Mobile.Contains(modifiedMobile)).FirstOrDefault();
                //var CustomerObj = unitofwork.customer.GetWhere(c => c.Mobile==mobile).FirstOrDefault();
                if (CustomerObj != null)
                {
                    string CustomerToken = unitofwork.LoginLog.GetWhere(L => L.FK_CustomerID == CustomerObj.ID).Select(L => L.Token).FirstOrDefault();
                    string ResetURL = ConfigurationManager.AppSettings["MobilPageURL"].ToString() + "/ResetPassword.aspx?ID=" + CustomerToken;
                    conf.SendSMSNew("Please visit this link: " + ResetURL + " to reset your Password.", mobile);
                }

                resp = new MessageObject { error = 0, message = "Success" };
            }
            catch (Exception)
            {
                resp = new MessageObject { error = 1, message = "Check internet connection" };
            }
            return resp;
        }

        [HttpGet]
        public MessageObject ConfirmForgetPassword(string token, string NewPassword)
        {
            MessageObject resp = new MessageObject();
            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(NewPassword))
                {
                    Customer CustomerObj = unitofwork.LoginLog.GetWhere(l => l.Token == token).Select(l => l.Customer).FirstOrDefault();
                    if (CustomerObj != null)
                    {
                        string Password = conf.GenerateToken(NewPassword);
                        CustomerObj.Password = Password;
                        unitofwork.Commit();
                        resp.error = 0;
                        resp.message = "Success";
                    }
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
        public CurrentPointsResponse CurrentPoints(string token)
        {
            CurrentPointsResponse resp = new CurrentPointsResponse();
            try
            {
                bool check = unitofwork.LoginLog.CheckTokenValidty(token);
                if (check == true)
                {
                    Customer CustomerObj = unitofwork.LoginLog.GetWhere(l => l.Token == token).Select(l => l.Customer).FirstOrDefault();
                    resp.CurrentPoints = CustomerObj.CurrentPoints ?? 0;
                    resp.error = 0;
                    resp.message = "Success";
                }
                else
                {
                    resp.error = 2;
                    resp.message = "Login again";
                }
            }
            catch (Exception)
            {
                resp.error = 1;
                resp.message = "Check internet connection";
            }
            return resp;
        }

        public void InsertLoginLog(string mobile)
        {
          //  string modifiedMobile = mobile.Substring(3, (mobile.Length - 3));
            var CustomerObj = unitofwork.customer.GetWhere(c => c.Mobile==mobile).FirstOrDefault();
            DateTime Today = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            string token = "";
            token = conf.GenerateToken(CustomerObj.ID.ToString());
            LoginLog Log = new LoginLog();
            Log.FK_CustomerID = CustomerObj.ID;
            Log.LogDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            Log.Token = token;

            unitofwork.LoginLog.Insert(Log);
            unitofwork.Commit();
        }

    }
}
