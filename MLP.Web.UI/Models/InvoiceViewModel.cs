using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLP.Web.UI.Models
{
    public class InvoiceViewModel
    {
        public int ID { get; set; }
        public Nullable<int> ServiceCenterID { get; set; }
        public string ServiceCenterName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Mobil { get; set; }

        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string CarNumber { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string PaymentType { get; set; }
        
        public Nullable<int> UserID { get; set; }
        public string UserName { get; set; }
        public Nullable<int> VisaLastDigits { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        
        
        public string CarCode { get; set; }
        public string CurrentMileage { get; set; }
        public string NextVisit { get; set; }
        public Nullable<int> LoyaltyPoints { get; set; }
        public Nullable<decimal> Vat { get; set; }
        public Nullable<decimal> VatPercent { get; set; }
        public Nullable<decimal> TotalWithVat { get; set; }

        public Nullable<decimal> Cash { get; set; }
        public Nullable<decimal> Visa { get; set; }
        public Nullable<decimal> Credit { get; set; }

        public Nullable<decimal> TotalReturn { get; set; }
        public Nullable<decimal> CashReturn { get; set; }
        public Nullable<decimal> VisaReturn { get; set; }
        public Nullable<decimal> CreditReturn { get; set; }
    }
}