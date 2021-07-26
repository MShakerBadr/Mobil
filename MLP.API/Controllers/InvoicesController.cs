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
    public class InvoicesController : ApiController
    {
        // GET api/invoices
        UnitOfWork unitofwork = new UnitOfWork();

        //public InvoicesListResponse Get(string token, string lang, string CarNum = null)
        //{
        //    bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);
        //    InvoicesListResponse resp = new InvoicesListResponse();
        //    if (checktoken == true)
        //    {
        //        try
        //        {
        //            resp.error = 0;
        //            resp.message = "Success";
        //            resp.Invoices = new List<InvoicesData>();

        //            int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();
        //            var CustomerObj = unitofwork.customer.GetById(CustomerID);
        //            var InvoicesList = unitofwork.Invoices.GetWhere(a => a.FK_CustomerCode == CustomerObj.CustomerCode);

        //            if (!string.IsNullOrEmpty(CarNum))
        //            {
        //                InvoicesList = InvoicesList.Where(x => x.CarNumber == CarNum);
        //            }

        //            foreach (var item in InvoicesList)
        //            {
        //                InvoicesData InvoiceObj = new InvoicesData();

        //                InvoiceObj.ID = item.ID;
        //                InvoiceObj.InvoiceNo = item.InvoiceNo ?? string.Empty;
        //                InvoiceObj.InvoiceDate = item.InvoiceDate.ToString() ?? string.Empty;
        //                InvoiceObj.ServiceCenter = unitofwork.ServiceCenter.GetWhere(x => x.IsActive == true && x.ID == item.FK_ServiceCenterID).Select(x => lang == "ar" ? x.CenterNameAr : x.CenterName).FirstOrDefault()?? string.Empty;
        //                InvoiceObj.CarNumber = item.CarNumber ?? string.Empty;
        //                InvoiceObj.TotalAmount = item.TotalAmount ?? 0;
        //                InvoiceObj.CurrentMileage = item.CurrentMileage ?? string.Empty;
        //                InvoiceObj.NextVisit = item.NextVisit ?? string.Empty;
        //                InvoiceObj.LoyaltyPoints = item.LoyaltyPoints ?? 0;
        //                InvoiceObj.TaxAmount = item.TaxAmount ?? 0;
        //                InvoiceObj.DiscountAmount = item.DiscountAmount ?? 0;

        //                resp.Invoices.Add(InvoiceObj);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            resp.error = 1;
        //            resp.message = "Check internet connection";
        //        }
        //    }
        //    else
        //    {
        //        resp.error = 2;
        //        resp.message = "Login again";
        //    }
        //    return resp;
        //}

        // GET api/invoices/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/invoices
        public InvoicesListResponse Post([FromBody] InvoiceParams Params)
        {
            InvoicesListResponse resp = new InvoicesListResponse();
            bool checktoken = unitofwork.LoginLog.CheckTokenValidty(Params.token);
            if (checktoken == true)
            {
                try
                {
                    resp.error = 0;
                    resp.message = "Success";
                    resp.Invoices = new List<InvoicesData>();

                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == Params.token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    var CustomerObj = unitofwork.customer.GetById(CustomerID);
                    var InvoicesList = unitofwork.Invoices.GetWhere(a => a.FK_CustomerCode == CustomerObj.CustomerCode);

                    if (!string.IsNullOrEmpty(Params.CarCode))
                    {
                        InvoicesList = InvoicesList.Where(x => x.FK_CarCode == Params.CarCode);
                    }

                    foreach (var Invoice in InvoicesList.ToList())
                    {
                        var InvoiceDetails = unitofwork.InvoiceDetails.GetWhere(x => x.FK_InvoiceID == Invoice.ID);
                        var ReturnInvoices = unitofwork.ReturnInvoices.GetWhere(x => x.FK_InvoiceNo == Invoice.InvoiceNo).ToList();
                        decimal TotalReturn = 0;
                        InvoicesData InvoiceObj = new InvoicesData();
                        InvoiceObj.ID = Invoice.ID;
                        InvoiceObj.InvoiceNo = Invoice.InvoiceNo ?? string.Empty;
                        InvoiceObj.InvoiceDate = Invoice.InvoiceDate.Value.ToString("dd/MM/yyyy HH:mm") ?? string.Empty;
                        InvoiceObj.ServiceCenter = unitofwork.ServiceCenter.GetWhere(x => x.IsActive == true && x.ID == Invoice.FK_ServiceCenterID).Select(x => Params.lang == "ar" ? x.CenterNameAr : x.CenterName).FirstOrDefault() ?? string.Empty;
                        InvoiceObj.CarCode = unitofwork.Vehicles.GetWhere(x => x.CarNumber == Invoice.FK_CarCode).Select(x => x.Car_Code).FirstOrDefault() ?? string.Empty;
                        InvoiceObj.CarNumber = unitofwork.Vehicles.GetWhere(x => x.CarNumber == Invoice.FK_CarCode).Select(x => x.CarNumber).FirstOrDefault() ?? string.Empty;
                        InvoiceObj.Model = unitofwork.Vehicles.GetWhere(x => x.CarNumber == Invoice.FK_CarCode).Select(x => x.model).FirstOrDefault() ?? string.Empty;
                        InvoiceObj.Vendor = unitofwork.Vehicles.GetWhere(x => x.CarNumber == Invoice.FK_CarCode).Select(x => x.vendor).FirstOrDefault() ?? string.Empty;
                        InvoiceObj.Motor = unitofwork.Vehicles.GetWhere(x => x.CarNumber == Invoice.FK_CarCode).Select(x => x.Motor).FirstOrDefault() ?? string.Empty;
                        var carInDb = unitofwork.Vehicles.GetWhere(x => x.CarNumber == Invoice.FK_CarCode).FirstOrDefault();
                        if (carInDb !=null)
                        {
                            if (carInDb.EngineType != null)
                            {
                                if (Params.lang == "ar")
                                    InvoiceObj.EngineType = carInDb.EngineType.Type_NameAr ?? string.Empty;
                                else
                                    InvoiceObj.EngineType = carInDb.EngineType.Type_Name ?? string.Empty;
                            }
                            else
                                InvoiceObj.EngineType = "";
                        }
                        else
                            InvoiceObj.EngineType = "";
                       
                        if (ReturnInvoices.Count > 0)
                            TotalReturn = ReturnInvoices.Select(x => x.TotalRetAmount.Value).DefaultIfEmpty().Sum();

                        //InvoiceObj.TotalAmount = Invoice.TotalAmount - TotalReturn ?? 0;
                        InvoiceObj.TotalAmount = Invoice.TotalWithVat - TotalReturn ?? 0;
                        InvoiceObj.CurrentMileage = Invoice.CurrentMileage ?? string.Empty;
                        InvoiceObj.NextVisit = Invoice.NextVisit ?? string.Empty;
                        InvoiceObj.LoyaltyPoints = CustomerObj.FirstLoginDate<Invoice.InvoiceDate?(Invoice.LoyaltyPoints??0 ): 0;
                        InvoiceObj.TaxAmount = Invoice.TaxAmount ?? 0;
                        InvoiceObj.DiscountAmount = Invoice.DiscountAmount ?? 0;
                        resp.Invoices.Add(InvoiceObj);

                        InvoiceObj.Details = new List<DetailsData>();

                        foreach (var DetailsItem in InvoiceDetails)
                        {
                            DetailsData DetailObj = new DetailsData();
                            if (DetailsItem.Price != 0)
                            {
                                DetailObj.IsBundle = string.IsNullOrEmpty(DetailsItem.FK_SalesItemID.ToString()) && !string.IsNullOrEmpty(DetailsItem.FK_BundleID.ToString());
                                if (DetailObj.IsBundle)
                                {
                                    DetailObj.ItemCode = string.Empty;
                                    DetailObj.ItemName = (Params.lang == "ar" ? unitofwork.Bundles.GetWhere(x => x.ID == DetailsItem.FK_BundleID).Select(x => x.BundleNameAr).FirstOrDefault() : unitofwork.Bundles.GetWhere(x => x.ID == DetailsItem.FK_BundleID).Select(x => x.BundleName).FirstOrDefault());
                                }
                                else
                                {
                                    DetailObj.ItemCode = DetailsItem.FK_SalesItemID == null ? string.Empty : unitofwork.SalesItem.GetWhere(x => x.ID == DetailsItem.FK_SalesItemID).Select(x => x.ItemCode).FirstOrDefault();
                                    DetailObj.ItemName = DetailsItem.FK_SalesItemID == null ? string.Empty : (Params.lang == "ar" ? unitofwork.SalesItem.GetWhere(x => x.ID == DetailsItem.FK_SalesItemID).Select(x => x.ItemNameAr).FirstOrDefault() : unitofwork.SalesItem.GetWhere(x => x.ID == DetailsItem.FK_SalesItemID).Select(x => x.ItemName).FirstOrDefault());
                                }

                                DetailObj.Qty = DetailsItem.Qty ?? 0;
                                DetailObj.Price = DetailsItem.Price ?? 0;
                                DetailObj.Total = DetailsItem.Price * DetailsItem.Qty ?? 0;

                                InvoiceObj.Details.Add(DetailObj);

                                DetailObj.Items = new List<ItemsData>();
                                var ItemsList = unitofwork.InvoiceDetails.GetWhere(x => x.FK_InvoiceID == Invoice.ID && DetailObj.IsBundle == true && x.Price == 0).ToList();

                                foreach (var item in ItemsList)
                                {
                                    ItemsData ItemObj = new ItemsData();
                                    ItemObj.ItemCode = item.FK_SalesItemID == null ? string.Empty : unitofwork.SalesItem.GetWhere(x => x.ID == item.FK_SalesItemID).Select(x => x.ItemCode).FirstOrDefault();
                                    ItemObj.ItemName = item.FK_SalesItemID == null ? string.Empty : (Params.lang == "ar" ? unitofwork.SalesItem.GetWhere(x => x.ID == item.FK_SalesItemID).Select(x => x.ItemNameAr).FirstOrDefault() : unitofwork.SalesItem.GetWhere(x => x.ID == item.FK_SalesItemID).Select(x => x.ItemName).FirstOrDefault());
                                    ItemObj.Qty = item.Qty ?? 0;
                                    ItemObj.Price = item.Price ?? 0;
                                    ItemObj.Total = item.Price * item.Qty ?? 0;

                                    DetailObj.Items.Add(ItemObj);
                                }
                            }
                            else if (DetailsItem.Price == 0 && DetailsItem.FK_SalesItemID != null && DetailsItem.FK_BundleID == null)
                            {
                                DetailObj.ItemCode = DetailsItem.FK_SalesItemID == null ? string.Empty : unitofwork.SalesItem.GetWhere(x => x.ID == DetailsItem.FK_SalesItemID).Select(x => x.ItemCode).FirstOrDefault();
                                DetailObj.ItemName = DetailsItem.FK_SalesItemID == null ? string.Empty : (Params.lang == "ar" ? unitofwork.SalesItem.GetWhere(x => x.ID == DetailsItem.FK_SalesItemID).Select(x => x.ItemNameAr).FirstOrDefault() : unitofwork.SalesItem.GetWhere(x => x.ID == DetailsItem.FK_SalesItemID).Select(x => x.ItemName).FirstOrDefault());

                                DetailObj.Qty = DetailsItem.Qty ?? 0;
                                DetailObj.Price = DetailsItem.Price ?? 0;
                                DetailObj.Total = DetailsItem.Price * DetailsItem.Qty ?? 0;
                                InvoiceObj.Details.Add(DetailObj);
                            }
                        }

                        foreach (var ReturnInvoiceItem in ReturnInvoices)
                        {
                            DetailsData DetailObj = new DetailsData();
                            var ReturnInvoiceDetails = unitofwork.ReturnInvoicesDetails.GetWhere(x => x.FK_RetInvoiceID == ReturnInvoiceItem.ID).ToList();
                            foreach (var item in ReturnInvoiceDetails)
                            {
                                DetailObj.ItemCode = item.FK_SalesItemID == null ? string.Empty : unitofwork.SalesItem.GetWhere(x => x.ID == item.FK_SalesItemID).Select(x => x.ItemCode).FirstOrDefault();
                                DetailObj.ItemName = item.FK_SalesItemID == null ? string.Empty : (Params.lang == "ar" ? unitofwork.SalesItem.GetWhere(x => x.ID == item.FK_SalesItemID).Select(x => x.ItemNameAr).FirstOrDefault() : unitofwork.SalesItem.GetWhere(x => x.ID == item.FK_SalesItemID).Select(x => x.ItemName).FirstOrDefault());

                                DetailObj.Qty = (item.ReturnQty * -1) ?? 0;
                                DetailObj.Price = (item.Price * -1) ?? 0;
                                DetailObj.Total = (item.Price * item.ReturnQty * -1) ?? 0;
                                InvoiceObj.Details.Add(DetailObj);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    resp.error = 1;
                    resp.message = "Check internet connection";
                }
            }
            else
            {
                resp.error = 2;
                resp.message = "Login again";
            }
            return resp;
        }

        // PUT api/invoices/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/invoices/5
        public void Delete(int id)
        {
        }
    }
}
