using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class RedeemReqResponse
    {
        public int error { get; set; }

        public string message { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Code { get; set; }

        public int CurrentPoints{ get; set; }

        public string AlfursanNo { get; set; }
    }
}
