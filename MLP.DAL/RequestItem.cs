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
    
    public partial class RequestItem
    {
        public int ID { get; set; }
        public Nullable<int> FK_SalesItemID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string FK_RequestNo { get; set; }
    
        public virtual SalesItem SalesItem { get; set; }
    }
}
