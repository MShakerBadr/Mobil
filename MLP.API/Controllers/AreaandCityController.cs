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
    public class AreaandCityController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();

        [HttpGet]

        public AreaCityResponse Getall(string lang)
        {
            AreaCityResponse resp;
            try
            {
                resp = new AreaCityResponse();
                resp.error = 0;
                resp.message = "Success";
                resp.data = new List<cities>();
                var obj = unitofwork.cities.GetAll().OrderBy(c => c.CityName);
                if (lang == "ar")
                {
                    obj = obj.OrderBy(c => c.CityNameAR);

                }
                else
                {
                    obj = obj.OrderBy(c => c.CityName);
                }
                foreach (var item in obj.ToList())
                {

                    cities c = new cities();

                    c.id = item.ID;
                    if (lang == "ar")
                        c.cityname = item.CityNameAR ?? string.Empty;
                    else
                        c.cityname = item.CityName ?? string.Empty;

                    List<CityArea> AreaList = new List<CityArea>();
                    var areas = item.Areas;
                    if (lang == "ar")
                    {
                        foreach (var item1 in item.Areas.OrderBy(a => a.AreaNameAR))
                        {
                            CityArea ca = new CityArea();
                            ca.id = item1.ID;
                            ca.AreaName = item1.AreaNameAR;
                            AreaList.Add(ca);
                        }
                    }
                    else
                    {
                        foreach (var item1 in item.Areas.OrderBy(a => a.AreaNameEN))
                        {
                            CityArea ca = new CityArea();
                            ca.id = item1.ID;
                            ca.AreaName = item1.AreaNameEN;
                            AreaList.Add(ca);
                        }

                    }

                    c.Areas = AreaList;

                    resp.data.Add(c);

                }
            }
            catch (Exception ex)
            {

                resp = new AreaCityResponse { error = 1, message = "Check internet connection" };
            }
            return resp;

        }

    }
}
