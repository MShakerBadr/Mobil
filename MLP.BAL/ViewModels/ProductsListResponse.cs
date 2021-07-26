using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class ProductsListResponse
    {
        public int error { get; set; }

        public string message { get; set; }

       // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public List<Products> data { get; set; }
    }

    public class Products
    {

        public int id { get; set; }

        public string productname { get; set; }

        public decimal itemprice { get; set; }

        public string description { get; set; }

        public string itemimage { get; set; }

        public string itemcode { get; set; }

        public string category { get; set; }

        public string DescHTML { get; set; }

      
    }
}
