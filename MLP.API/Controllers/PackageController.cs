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
    public class PackageController : ApiController
    {
        // GET api/package
        UnitOfWork unitofwork = new UnitOfWork();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/package/5
        public PackageListResponse Get(string lang)
        {
            PackageListResponse resp;
            try
            {
                resp = new PackageListResponse();
                resp.error = 0;
                resp.message = "Success";
                resp.data = new List<PackagesList>();

                var NewsObjList = unitofwork.Packages.GetWhere(x => x.IsActive == true).ToList().OrderByDescending(x => x.CreationDate);
                foreach (var item in NewsObjList)
                {
                    PackagesList obj = new PackagesList();
                    obj.ID = item.ID;
                    obj.PackageDate = //((DateTime)item.PackageDate).ToString("dd/MM/yyyy") ?? 
                    string.Empty;
                    obj.PackageImage = item.PackageImage ?? string.Empty;

                    if (lang == "ar")
                    {
                        obj.PackageTitle = item.TitleAr ?? string.Empty;
                        obj.PackageAbtract = item.PackageAbtractAr ?? string.Empty;
                        obj.PackageHTML = item.PackageHTMLAr ?? string.Empty;
                    }
                    else
                    {
                        obj.PackageTitle = item.Title ?? string.Empty;
                        obj.PackageAbtract = item.PackageAbtract ?? string.Empty;
                        obj.PackageHTML = item.PackageHTML ?? string.Empty;
                    }

                    resp.data.Add(obj);
                }
            }
            catch (Exception)
            {

                resp = new PackageListResponse { error = 1, message = "Check internet connection" };
            }

            return resp;
        }

        // POST api/package
        public void Post([FromBody]string value)
        {
        }

        // PUT api/package/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/package/5
        public void Delete(int id)
        {
        }
    }
}
