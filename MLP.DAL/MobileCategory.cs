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
    
    public partial class MobileCategory
    {
        public MobileCategory()
        {
            this.SalesItems = new HashSet<SalesItem>();
        }
    
        public int ID { get; set; }
        public string ProductCategory { get; set; }
        public string ProductCategoryAr { get; set; }
        public string CategoryImage { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> FK_CreatorID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    
        public virtual ICollection<SalesItem> SalesItems { get; set; }
    }
}