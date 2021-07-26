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
    public class NewsController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();

        List<News> NewsList = new List<News>();
        // GET api/news
        public NewsController() { }

        public NewsController(List<News> NewsList)
        {
            this.NewsList = NewsList;
        }

        public IEnumerable<News> GetAllNews()
        {
            return NewsList;
        }

        public NewsListResponse Get(string lang)
        {
            NewsListResponse resp;
            try
            {
                resp = new NewsListResponse();
                resp.error = 0;
                resp.message = "Success";
                resp.data = new List<NewsList>();

                var NewsObjList = unitofwork.News.GetWhere(x => x.IsActive == true).OrderByDescending(x => x.NewsDate).ToList();
                foreach (var item in NewsObjList)
                {
                    NewsList obj = new NewsList();
                    obj.ID = item.ID;
                    obj.NewsDate = ((DateTime)item.NewsDate).ToString("dd/MM/yyyy") ?? string.Empty;
                    obj.NewsImage = item.NewsImage ?? string.Empty;

                    if (lang == "ar")
                    {
                        obj.NewsTitle = item.TitleAr ?? string.Empty;
                        obj.NewsAbtract = item.NewsAbtractAr ?? string.Empty;
                        obj.NewsHTML = item.NewsHTMLAr ?? string.Empty;
                    }
                    else
                    {
                        obj.NewsTitle = item.Title ?? string.Empty;
                        obj.NewsAbtract = item.NewsAbtract ?? string.Empty;
                        obj.NewsHTML = item.NewsHTML ?? string.Empty;
                    }

                    resp.data.Add(obj);
                }
            }
            catch (Exception)
            {

                resp = new NewsListResponse { error = 1, message = "Check internet connection" };
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
