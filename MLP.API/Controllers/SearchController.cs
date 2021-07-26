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
    public class SearchController : ApiController
    {
        // GET api/search
        UnitOfWork unitofwork = new UnitOfWork();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/search/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/search
        public HttpResponseMessage Post([FromBody]SearchParams Params, string token, string lang, int SearchType, int Sortby)
        {
            try
            {
                bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);

                if (checktoken == true)
                {
                    if (SearchType == 1) // Products
                    {
                        ProductsListResponse resp = new ProductsListResponse();

                        resp.error = 0;
                        resp.message = "Success";


                        resp.data = new List<Products>();

                        var Prod = unitofwork.SalesItem.GetWhere(p => p.IsActive == true & p.IsMobileProduct == true &
                            (
                            (p.ItemName.Contains(Params.Searchkeyword)) || (p.ItemNameAr.Contains(Params.Searchkeyword)) ||(p.Description.Contains(Params.Searchkeyword)) || (p.DescriptionHTML.Contains(Params.Searchkeyword)) ||
                             (p.MobileCategory.ProductCategory.Contains(Params.Searchkeyword)) || (p.MobileCategory.ProductCategoryAr.Contains(Params.Searchkeyword)))
                            ).OrderBy(p => p.ItemName).ToList();

                        foreach (var item in Prod)
                        {
                            Products pp = new Products();
                            pp.id = item.ID;
                            pp.itemcode = item.ItemCode ?? string.Empty;
                            pp.itemimage = item.ImageName ?? string.Empty;
                            pp.itemprice = item.ItemPrice ?? 0;
                            pp.DescHTML = item.DescriptionHTML ?? string.Empty;
                            if (lang != "ar")
                            {
                                pp.productname = item.ItemName;
                                pp.description = item.Description ?? string.Empty;
                                pp.category = item.MobileCategory.ProductCategory;
                            }
                            else
                            {
                                pp.productname = item.ItemNameAr;
                                pp.description = item.Description ?? string.Empty;
                                pp.category = item.MobileCategory.ProductCategoryAr ?? string.Empty;
                            }

                            resp.data.Add(pp);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, resp);
                    }
                    else if (SearchType == 2) // Branches
                    {
                        ServiceCenterListResponse resp = new ServiceCenterListResponse();

                        if (Sortby == 1 || Sortby == 2)
                        {
                            resp = unitofwork.ServiceCenter.GetServiceCenters(Sortby, lang, Params.Searchkeyword);
                        }
                        else if (Sortby == 3)
                        {
                            resp = unitofwork.ServiceCenter.GetServiceCenters(Sortby, Params.latitude, Params.longitude, lang, Params.Searchkeyword);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, resp);
                    }
                }
                else
                {
                    MessageObject resp = new MessageObject();
                    resp.error = 2;
                    resp.message = "Login again";
                    return Request.CreateResponse(HttpStatusCode.NotFound, resp);
                }
            }
            catch (Exception ex)
            {
                MessageObject resp = new MessageObject();
                resp.error = 1;
                resp.message = "Check internet connection";
                return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/search/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/search/5
        public void Delete(int id)
        {
        }
    }
}
