using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class InvoicesListResponse
    {
        public int error { get; set; }

        public string message { get; set; }

       // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<InvoicesData> Invoices { get; set; }
    }
    public class InvoicesData
    {
        public int ID { get; set; }
        public string InvoiceNo { get; set; }
        public decimal TotalAmount { get; set; }
        public string InvoiceDate { get; set; }
        public string ServiceCenter { get; set; }
        public string CarNumber { get; set; }
        public string CarCode { get; set; }
        public string  Model { get; set; }
        public string Vendor { get; set; }
        public string Motor { get; set; }
        public string CurrentMileage { get; set; }
        public string NextVisit { get; set; }
        public int LoyaltyPoints { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string EngineType { get; set; }
        public List<DetailsData> Details { get; set; }

    }

    public class DetailsData
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public bool IsBundle { get; set; }

        public List<ItemsData> Items { get; set; }

    }

    public class ItemsData
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
