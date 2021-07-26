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
    public class ServiceCentersController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();

        [HttpPost]
        public ServiceCenterListResponse GetBySorttype(int Sortby, [FromBody]SortBody Param, string lang)
        {
            ServiceCenterListResponse resp;
            try
            {
                if (Sortby == 1 || Sortby == 2)
                {
                    resp = unitofwork.ServiceCenter.GetServiceCenters(Sortby, lang);
                    
                }
                else if (Sortby == 3)
                {
                    resp = unitofwork.ServiceCenter.GetServiceCenters(Sortby, Param.latitude, Param.longitude, lang);
                }
                else
                {
                    resp = new ServiceCenterListResponse { error = 7, message = "Wrong or missing Parameters" };
                }
            }
            catch (Exception ex)
            {

                resp = new ServiceCenterListResponse { error = 1, message = "Check internet connection" };
            }
            return resp;

        }

        /* Get All Service Centers With IsBooking Flag True */
        [HttpPost]
        public ServiceCenterListResponse GetBookingBranch(int Sortby, [FromBody]SortBody Param, string lang)
        {
            ServiceCenterListResponse resp;
            try
            {
                if (Sortby == 1 || Sortby == 2)
                {
                    resp = unitofwork.ServiceCenter.GetBookingServiceCenters(Sortby, lang);
                }
                else if (Sortby == 3)
                {
                    resp = unitofwork.ServiceCenter.GetBookingServiceCenters(Sortby, Param.latitude, Param.longitude, lang);
                }
                else
                {
                    resp = new ServiceCenterListResponse { error = 7, message = "Wrong or missing Parameters" };
                }
            }
            catch (Exception ex)
            {

                resp = new ServiceCenterListResponse { error = 1, message = "Check internet connection" };
            }
            return resp;
        }
        [HttpGet]
        public ServiceResponse GetServices(string lang, int ServiceCenterID)
        {
            ServiceResponse resp = new ServiceResponse();
            try
            {
                resp.error = 0; resp.message = "Success";
                resp.data = new List<Services>();

                var ItemIDs = unitofwork.ServiceCenterSalesItems.GetWhere(s => s.FK_ServiceCenterID == ServiceCenterID).Select(s => s.FK_ItemID).ToList();
                var Services = unitofwork.SalesItem.GetWhere(s => s.IsActive == true && s.FK_ItemTypeID == 2 && ItemIDs.Contains(s.ID) && s.IsMobileProduct == true).OrderBy(s => s.MobileDisplayOrder).ToList();
                if (Services.Count > 0)
                {
                    foreach (var item in Services)
                    {
                        Services Ser = new Services();
                        Ser.ID = item.ID;
                        Ser.SerivceImage = item.ImageName ?? string.Empty;
                        Ser.Price = item.ItemPrice ?? 0;
                        Ser.DescHTML = item.DescriptionHTML ?? string.Empty;
                        Ser.Description = item.Description ?? string.Empty;

                        if (lang != "ar")
                            Ser.SerivceName = item.ItemName;

                        else
                            Ser.SerivceName = item.ItemNameAr;

                        resp.data.Add(Ser);
                    }

                }
            }
            catch (Exception ex)
            {

                resp.error = 1; resp.message = "Check internet connection";
            }
            return resp;
        }

    }
}
