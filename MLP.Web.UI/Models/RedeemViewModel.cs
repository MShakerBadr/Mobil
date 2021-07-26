using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLP.Web.UI.Models
{
    public class RedeemViewModel
    {
        public int ID { get; set; }
        public string Customer { get; set; }
        public string Award { get; set; }
        public string ServiceCenter { get; set; }
        public int Qty { get; set; }
        public string RedeemDate { get; set; }
        public int RedeemPoints { get; set; }
        public int FK_ServiceCenterID { get; set; }
    }
}