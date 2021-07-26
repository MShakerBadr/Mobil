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
    public class AwardsController : ApiController
    {
        // GET api/awardslist
        UnitOfWork unitofwork = new UnitOfWork();

        public AwardsListResponse Get(string lang, string token)
        {
            bool checktoken = unitofwork.LoginLog.CheckTokenValidty(token);
            AwardsListResponse resp = new AwardsListResponse();
            if (checktoken == true)
            {
                try
                {
                    resp.error = 0;
                    resp.message = "Success";
                    resp.Awards = new List<AwardsData>();

                    int CustomerID = unitofwork.LoginLog.GetWhere(x => x.Token == token).Select(x => x.FK_CustomerID).FirstOrDefault();
                    var CustomerObj = unitofwork.customer.GetById(CustomerID);
                    var AwardsList = unitofwork.Awards.GetWhere(a => a.IsActive == true);

                    foreach (var item in AwardsList)
                    {
                        AwardsData award = new AwardsData();

                        award.ID = item.ID;
                        award.AwardImage = item.AwardImage ?? string.Empty;
                        award.Product = unitofwork.SalesItem.GetWhere(x => x.ID == item.FK_SalesItemID).Select(x => lang == "ar" ? x.ItemNameAr : x.ItemName).FirstOrDefault();
                        award.Qty = item.Qty ?? 0;
                        award.AwardDuePoints = item.AwardDuePoints ?? 0;
                        //award.Level = unitofwork.Levels.GetWhere(l => l.ID == item.FK_LevelID).Select(l => lang == "ar" ? l.LevelAR : l.LevelEng).FirstOrDefault();
                        award.AwardTypeID = item.FK_AwardTypeID;
                        award.AwardTypeName = unitofwork.AwardsTypes.GetWhere(x => x.ID == item.FK_AwardTypeID).Select(x => x.AwardType).FirstOrDefault();
                        if (CustomerObj.CurrentPoints >= item.AwardDuePoints)
                            award.Enabled = true;
                        else
                            award.Enabled = false;

                        resp.Awards.Add(award);
                    }

                    resp.Awards = resp.Awards.OrderByDescending(x => x.Enabled).OrderBy(x=>x.AwardDuePoints).OrderByDescending(x => x.AwardTypeID).ToList();
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

        // GET api/awardslist/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/awardslist
        public void Post([FromBody]string value)
        {
        }

        // PUT api/awardslist/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/awardslist/5
        public void Delete(int id)
        {
        }
    }
}
