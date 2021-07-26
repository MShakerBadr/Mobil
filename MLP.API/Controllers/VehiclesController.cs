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
    public class VehiclesController : ApiController
    {
        // GET api/vehicles
        UnitOfWork unitofwork = new UnitOfWork();
        public VehiclesListResponse Get(string token, string lang)
        {
            bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);
            VehiclesListResponse resp = new VehiclesListResponse();
            if (checktoken == true)
            {
                try
                {
                    resp.error = 0;
                    resp.message = "Success";
                    resp.Vehicles = new List<VehiclesData>();

                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    var CustomerObj = unitofwork.customer.GetById(CustomerID);
                    var VehiclesList = unitofwork.Vehicles.GetWhere(a => a.FK_CustomerCode == CustomerObj.CustomerCode);

                    foreach (var item in VehiclesList)
                    {
                        VehiclesData VehicleObj = new VehiclesData();
                        VehicleObj.ID = item.ID;
                        VehicleObj.CarCode = item.Car_Code ?? string.Empty;
                        VehicleObj.CarNumber = item.CarNumber ?? string.Empty;
                        VehicleObj.Model = item.model ?? string.Empty;
                        VehicleObj.Vendor = item.vendor ?? string.Empty;
                        VehicleObj.Year = item.year ?? string.Empty;
                        VehicleObj.Motor = item.Motor ?? string.Empty;
                        if (item.EngineType != null)
                        {
                            if (lang == "ar")
                                VehicleObj.EngineType = item.EngineType.Type_NameAr ?? string.Empty;
                            else
                                VehicleObj.EngineType = item.EngineType.Type_Name ?? string.Empty;
                        }
                        else
                            VehicleObj.EngineType = "";
                        resp.Vehicles.Add(VehicleObj);
                    }
                }
                catch (Exception)
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

        // GET api/vehicles/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/vehicles
        public void Post([FromBody]string value)
        {
        }

        // PUT api/vehicles/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/vehicles/5
        public void Delete(int id)
        {
        }
    }
}
