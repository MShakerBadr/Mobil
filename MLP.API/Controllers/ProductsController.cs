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
    public class ProductsController : ApiController
    {
        UnitOfWork unitofwork = new UnitOfWork();

        [HttpGet]
        public ProductsListResponse Getallproducts(string lang)
        {
            ProductsListResponse resp;
            try
            {
                resp = new ProductsListResponse();

                resp.error = 0;
                resp.message = "Success";


                resp.data = new List<Products>();

                var Prod = unitofwork.SalesItem.GetWhere(p => p.IsActive == true & p.IsMobileProduct == true && p.FK_ItemTypeID == 1).OrderBy(p => p.MobileDisplayOrder).ToList();

                if (Prod.Count > 0)
                {
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
                            pp.category = item.MobileCategory.ProductCategory ?? string.Empty;
                        }
                        else
                        {
                            pp.productname = item.ItemNameAr;
                            pp.description = item.Description ?? string.Empty;
                            pp.category = item.MobileCategory.ProductCategoryAr ?? string.Empty;
                        }

                        resp.data.Add(pp);
                    }

                }

            }
            catch (Exception)
            {

                resp = new ProductsListResponse { error = 1, message = "Check internet connection" };
            }
            return resp;

        }

        [HttpGet]

        public CategoryListResponse GetAllCategories()
        {

            CategoryListResponse resp;
            try
            {

                resp = new CategoryListResponse();
                resp.error = 0;
                resp.message = "Success";

                resp.data = new List<Categ>();

                var data = unitofwork.MobileCategory.GetAll().ToList();

                foreach (var item in data)
                {
                    Categ c = new Categ();
                    c.id = item.ID;
                    c.categoryname = item.ProductCategory ?? string.Empty;
                    c.categorynamear = item.ProductCategoryAr ?? string.Empty;

                    resp.data.Add(c);

                }
            }
            catch (Exception)
            {


                resp = new CategoryListResponse { error = 1, message = "Check internet connection" };
            }
            return resp;
        }

    }
}
