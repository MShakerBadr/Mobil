using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    
    public class LoginResponse
    {
        public int error { get; set; }

        public string message { get; set; }

      //  [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Token { get; set; }
        public DateTime LoginDate { get; set; }
        public UserData data { get; set; }
    }

    public class UserData
    {
        public string usercode { get; set; }

        public string fname { get; set; }

        public string lname { get; set; }

        public string brithdate { get; set; }

        public string mobile { get; set; }

        public string mail { get; set; }

        public string address { get; set; }

        public string area { get; set; }

        public string city { get; set; }
        public string areaar { get; set; }

        public string cityar { get; set; }

        public string levelname { get; set; }

        public int points { get; set; }

        public string gender { get; set; }
        public int BarLevel { get; set; }
        public bool IsFristLogin { get; set; }
        public int? CityId { get; set; }
        public int? AreaId { get; set; }

        public string AlfursanNo { get; set; }
    }
}
