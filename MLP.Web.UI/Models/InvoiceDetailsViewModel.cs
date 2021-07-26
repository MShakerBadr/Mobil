using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLP.Web.UI.Models
{
    public class InvoiceDetailsViewModel
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> OriginalPrice { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string IsPackage { get; set; }

    }
}