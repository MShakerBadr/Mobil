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
    
    public partial class AuditingItem
    {
        public int ID { get; set; }
        public Nullable<int> FK_AuditingID { get; set; }
        public Nullable<int> FK_SalesItemID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> InventoryQuantity { get; set; }
        public Nullable<int> DifferenceQuantity { get; set; }
    }
}
