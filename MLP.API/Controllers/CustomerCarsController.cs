using MLP.BAL;
using MLP.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MLP.DAL;
using Newtonsoft.Json;

namespace MLP.API.Controllers
{
    public class CustomerCarsController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();
        [HttpGet]
        // GET api/customercars
        public HttpResponseMessage Get(string lang)
        {
            CarLookupViewModel carLookup = new CarLookupViewModel();
            try
            {
                var Engine = unitofwork.EngineType.GetAll().Where(e => e.IsMobile == true && e.IsActive == true).OrderBy(e => e.Type_Name).OrderBy(e => e.Type_NameAr).ToList();
                foreach (var item in Engine)
                {
                    EngineTypeVM engineType = new EngineTypeVM();
                    engineType.Id = item.Engine_ID;
                    if (lang == "ar")
                        engineType.EngineType = item.Type_NameAr;
                    else
                        engineType.EngineType = item.Type_Name;
                    carLookup.EngineTypes.Add(engineType);
                }
                var Breand = unitofwork.CarBrand.GetAll();
                if (lang == "ar")
                {
                    Breand = Breand.OrderBy(b => b.BrandNameAR);

                    foreach (var item in Breand.ToList())
                    {
                        CarBrandVm carBrand = new CarBrandVm();
                        carBrand.Id = item.ID;
                        carBrand.CarBrand = item.BrandNameAR;
                        foreach (var item2 in item.CarModels.OrderBy(m => m.ModelNameAR))
                        {
                            CarModelVm carModel = new CarModelVm();
                            carModel.Id = item2.ID;
                            carModel.CarModel = item2.ModelNameAR;
                            carBrand.CarModels.Add(carModel);
                        }
                        carLookup.CarBrands.Add(carBrand);
                    }
                }
                else
                {
                    Breand = Breand.OrderBy(b => b.BrandName);
                    foreach (var item in Breand.ToList())
                    {
                        CarBrandVm carBrand = new CarBrandVm();
                        carBrand.Id = item.ID;
                        carBrand.CarBrand = item.BrandName;
                        foreach (var item2 in item.CarModels.OrderBy(m => m.ModeName))
                        {
                            CarModelVm carModel = new CarModelVm();
                            carModel.Id = item2.ID;
                            carModel.CarModel = item2.ModeName;
                            carBrand.CarModels.Add(carModel);
                        }
                        carLookup.CarBrands.Add(carBrand);
                    }
                }
                carLookup.error = 0;
                carLookup.message = "Success";
                return Request.CreateResponse(HttpStatusCode.OK, carLookup);
            }
            catch (Exception)
            {
                carLookup.error = 1;
                carLookup.message = "Server Error";
                return Request.CreateResponse(HttpStatusCode.BadRequest, carLookup);
            }

        }

        // GET api/customercars/5
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost]
        // POST api/customercars
        public HttpResponseMessage Post([FromBody] CarViewModel carvm)
        {
            if (ModelState.IsValid)
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(carvm.token);
                if (checktoken == true)
                {
                    carvm.error = 0;
                    carvm.message = "Success";
                    var customerID = unitofwork.LoginLog.GetCustomerIdByToken(carvm.token);
                    var CustomerObj = unitofwork.customer.GetById(customerID);
                    var ModelObj = unitofwork.CarModel.GetById(carvm.modelId);
                    var BrandObj = unitofwork.CarBrand.GetById(carvm.BrandId);
                    CustomersCarNumber car = new CustomersCarNumber();
                    car.CarNumber = carvm.CarNumber;
                    car.CreationDate = DateTime.Now;
                    car.FK_CustomerCode = CustomerObj.CustomerCode;
                    car.LastModifiedDate = DateTime.Now;
                    car.model = ModelObj.ModeName;
                    car.Motor = carvm.Motor;
                    car.vendor = BrandObj.BrandName;
                    car.year = carvm.Year;
                    car.FK_Engine_Type = carvm.EngineTypeId;
                    bool result = false;
                    //if (carvm.Id == 0)
                    //{
                    result = unitofwork.customerCars.Insert(car);
                    unitofwork.Commit();

                    //}
                    //else
                    //{
                    //    car.ID = carvm.Id;
                    //    var carIndb = unitofwork.customerCars.GetById(carvm.Id);
                    //    if (carIndb != null)
                    //    {
                    //        unitofwork.customerCars.Update(car);
                    //        result = true;
                    //        unitofwork.Commit();
                    //    }
                    //    else
                    //    {
                    //        return Request.CreateResponse(HttpStatusCode.NotFound, carvm);
                    //    }
                    //}}


                    if (result)
                    {
                        carvm.Id = unitofwork.customerCars.GetAll().Max(c => c.ID);
                        return Request.CreateResponse(HttpStatusCode.Created, carvm);
                    }
                    else
                    {
                        carvm.error = 1;
                        carvm.message = "Check internet connection";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, carvm);
                    }
                }
                else
                {

                    carvm.error = 2;
                    carvm.message = "Login again";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, carvm);
                }
            }
            else
            {
                carvm.error = 1;
                carvm.message = "Server Error";
                return Request.CreateResponse(HttpStatusCode.BadRequest, carvm);
            }
        }

        // PUT api/customercars/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/customercars/5
        public void Delete(int id)
        {
        }
    }
}
