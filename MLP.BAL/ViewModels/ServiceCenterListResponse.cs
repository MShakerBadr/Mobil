using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class ServiceCenterListResponse
    {

        public int error { get; set; }

        public string message { get; set; }

       // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ServiceCenters> data { get; set; }
    }

    public class ServiceCenters
    {
        public int id { get; set; }

        public string centername { get; set; }

        //public string centernamear { get; set; }

        public string address { get; set; }

        //public string addressar { get; set; }

        public string city { get; set; }

        //public string cityar { get; set; }

        public string area { get; set; }

        public string image { get; set; }

        //public string areaar { get; set; }

        public string branchdetails { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }

        public int cityid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public double distance { get; set; }

        public string DescHTML { get; set; }

        public bool IsLastBranch { get; set; }

        public bool IsFullday { get; set; }

        public string OpeningTime { get; set; }

        public string ClosingTime { get; set; }

        public string ContactNo { get; set; }
        public bool  comingSoon { get; set; }

    }
}
