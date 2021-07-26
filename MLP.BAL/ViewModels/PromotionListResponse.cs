using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{

    public class PromotionListResponse
    {
        public int error { get; set; }

        public string message { get; set; }
       // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Promotionsdata> data { get; set; }
    }

    public class Promotionsdata
    {
        public int id { get; set; }
        public string description { get; set; }

        public string title { get; set; }

        public string imge { get; set; }

        public string startdate { get; set; }

        public string enddate { get; set; }

        public decimal price { get; set; }

        public string DescHTML { get; set; }
        public string status { get; set; }
        public string color { get; set; }

    }
}
