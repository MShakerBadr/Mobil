//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MLP.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoicePayment
    {
        public int ID { get; set; }
        public Nullable<int> FK_PaymentTypeID { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<int> FK_InvoiceID { get; set; }
        public Nullable<int> FK_InvoiceTypeID { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
