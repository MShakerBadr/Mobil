using MLP.BAL;
using MLP.BAL.ViewModels;
using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MLP.API.Controllers
{
    public class AdsController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();

      
        // GET api/news
        public AdsController() { }


      
        public AdvertisementResponse Get()
        {
            AdvertisementResponse resp;
            try
            {
                resp = new AdvertisementResponse();
                resp.error = 0;
                resp.message = "Success";
                resp.data = new List<AdvertisementsData>();

                var AdsObjList = unitofwork.Ads.GetWhere(x => x.IsActive == true).OrderByDescending(x => x.Lastmodifieddate).ToList();
                foreach (var item in AdsObjList)
                {
                    AdvertisementsData obj = new AdvertisementsData();
                    obj.ID = item.ID;
                    obj.Lastmodifieddate = ((DateTime)item.Lastmodifieddate).ToString("dd/MM/yyyy") ?? string.Empty;
                    obj.CreationDate = ((DateTime)item.CreationDate).ToString("dd/MM/yyyy") ?? string.Empty;
                    obj.Image = item.Imagename ?? string.Empty;
                    obj.Order= item.AdsOrder ?? 0;
                   
                    resp.data.Add(obj);
                }
            }
            catch (Exception)
            {

                resp = new AdvertisementResponse { error = 1, message = "Check internet connection" };
            }

            return resp;
        }

        // GET api/news/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/news
        public void Post([FromBody]string value)
        {
        }

        // PUT api/news/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/news/5
        public void Delete(int id)
        {
        }
    }
}
