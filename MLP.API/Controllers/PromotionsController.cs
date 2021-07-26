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
    public class PromotionsController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();

        [HttpGet]
        public PromotionListResponse Getallpromtions(string lang)
        {
            PromotionListResponse resp;
            try
            {
                resp = new PromotionListResponse();

                resp.error = 0;
                resp.message = "Success";


                resp.data = new List<Promotionsdata>();

                var Prod = unitofwork.Promotion.GetWhere(p => p.IsActive == true).OrderByDescending(p => p.EndDate).ToList();

                if (Prod.Count > 0)
                {
                    foreach (var item in Prod)
                    {
                        Promotionsdata pd = new Promotionsdata();
                        pd.id = item.ID;
                        if (lang == "ar")
                        {
                            pd.imge = item.ImageAR ?? string.Empty;
                        }
                        else
                        {
                            pd.imge = item.ImageEng ?? string.Empty;
                        }
                        pd.price = item.Price ?? 0;
                        pd.startdate = item.StartDate.Value.ToString("dd/MM/yyyy").Replace("-", "/") ?? string.Empty;
                        pd.enddate = item.EndDate.Value.ToString("dd/MM/yyyy").Replace("-", "/") ?? string.Empty;
                        if (item.EndDate.Value.Date < DateTime.Now.Date)
                        {
                            if (lang == "ar")
                                pd.status = "منتهى";
                            else
                                pd.status = "Expired";
                            pd.color = "#ff0000"; //red
                        }
                        else if (item.StartDate.Value.Date <= DateTime.Now.Date && item.EndDate.Value.Date >= DateTime.Now.Date)
                        {
                            if (lang == "ar")
                                pd.status = "فعال";
                            else
                            pd.status = "Active";
                            pd.color = "#00ff00"; // Green
                        }
                        else if (item.StartDate.Value.Date > DateTime.Now.Date)
                        {
                            if (lang == "ar")
                                pd.status = "قادم";
                            else
                            pd.status = "Upcoming";
                            pd.color = "#ffbf00"; //Yellow
                        }

                        pd.DescHTML = item.DescriptionHTML ?? string.Empty;
                        if (lang == "ar")
                        {
                            pd.title = item.TitleAR ?? string.Empty;
                            pd.description = item.DescAR ?? string.Empty;
                        }
                        else
                        {
                            pd.description = item.DescEng ?? string.Empty;
                            pd.title = item.TitleEng ?? string.Empty;
                        }

                        resp.data.Add(pd);
                    }

                }



            }
            catch (Exception)
            {

                resp = new PromotionListResponse { error = 1, message = "Check internet connection" };
            }
            return resp;

        }
    }
}
