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
    
    public partial class SalesItemCategory
    {
        public SalesItemCategory()
        {
            this.SalesItems = new HashSet<SalesItem>();
        }
    
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameAr { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> FK_UserID { get; set; }
    
        public virtual ICollection<SalesItem> SalesItems { get; set; }
    }
}