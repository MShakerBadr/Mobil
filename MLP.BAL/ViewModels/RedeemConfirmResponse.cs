using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class RedeemConfirmResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public int CurrentPoints { get; set; }

        public List<RedeemData> Data { get; set; }

    }

    public class RedeemData
    {
        public int ID { get; set; }

        public string Award { get; set; }

        public string Image { get; set; }

        public string ServiceCenter { get; set; }

        public string RedeemDate { get; set; }

        public int RedeemPoints { get; set; }


    }
}
