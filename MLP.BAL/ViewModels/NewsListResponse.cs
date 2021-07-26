using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class NewsListResponse
    {
        public int error { get; set; }

        public string message { get; set; }

       // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public List<NewsList> data { get; set; }
    }
    public class NewsList
    {
        public int ID { get; set; }
        public string NewsTitle { get; set; }
        public string NewsAbtract { get; set; }
        public string NewsHTML { get; set; }
        public string NewsImage { get; set; }
        public string NewsDate { get; set; }
    }
}
