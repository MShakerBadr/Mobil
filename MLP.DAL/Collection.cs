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
    
    public partial class Collection
    {
        public int ID { get; set; }
        public string ConfirmationCode { get; set; }
        public Nullable<System.DateTime> SendingDate { get; set; }
        public Nullable<decimal> AmountRequired { get; set; }
        public Nullable<decimal> ActualAmount { get; set; }
        public Nullable<int> FK_ServiceCenterID { get; set; }
        public string CollectorCode { get; set; }
        public Nullable<int> FK_UserID { get; set; }
        public Nullable<System.DateTime> CollectionDate { get; set; }
        public bool IsCollected { get; set; }
        public Nullable<bool> HasProblem { get; set; }
    }
}