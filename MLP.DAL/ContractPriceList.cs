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
    
    public partial class ContractPriceList
    {
        public int ID { get; set; }
        public Nullable<int> FK_ContractID { get; set; }
        public Nullable<int> FK_SalesItemID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int CreatorID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<decimal> Vat { get; set; }
    }
}
