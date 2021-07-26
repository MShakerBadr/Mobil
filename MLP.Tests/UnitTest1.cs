using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MLP.API.Controllers;
using MLP.DAL;

namespace MLP.Tests
{
    [TestClass]
    public class TestNewsController
    {
        [TestMethod]
        public void GetAllNews_ShouldReturnAllNews()
        {
            var testNews = GetTestNews();
            var controller = new NewsController(testNews);

            var result = controller.GetAllNews() as List<News>;
            Assert.AreEqual(testNews.Count, result.Count);
        }

        private List<News> GetTestNews()
        {
            var testNews = new List<News>();
            testNews.Add(new News { ID = 1, Title = "Test Title1", TitleAr = "Test Title Ar1", NewsAbtract = "Test Abstract1", NewsAbtractAr = "Test Abstract Ar1", NewsHTML = "<html></html>", NewsDate = DateTime.Now, NewsImage = "~/Images/Image01.png", CreationDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsActive = true, FK_CreatorID = 1 });
            testNews.Add(new News { ID = 2, Title = "Test Title2", TitleAr = "Test Title Ar2", NewsAbtract = "Test Abstract2", NewsAbtractAr = "Test Abstract Ar2", NewsHTML = "<html></html>", NewsDate = DateTime.Now, NewsImage = "~/Images/Image02.png", CreationDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsActive = true, FK_CreatorID = 1 });
            testNews.Add(new News { ID = 3, Title = "Test Title3", TitleAr = "Test Title Ar3", NewsAbtract = "Test Abstract3", NewsAbtractAr = "Test Abstract Ar3", NewsHTML = "<html></html>", NewsDate = DateTime.Now, NewsImage = "~/Images/Image03.png", CreationDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsActive = true, FK_CreatorID = 1 });
            testNews.Add(new News { ID = 4, Title = "Test Title4", TitleAr = "Test Title Ar4", NewsAbtract = "Test Abstract4", NewsAbtractAr = "Test Abstract Ar4", NewsHTML = "<html></html>", NewsDate = DateTime.Now, NewsImage = "~/Images/Image04.png", CreationDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsActive = true, FK_CreatorID = 1 });

            return testNews;
        }
    }
}
