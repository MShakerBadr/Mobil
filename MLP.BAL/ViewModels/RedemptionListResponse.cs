using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class RedemptionListResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<RedeemData> Data { get; set; }
    }
}
