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
    
    public partial class Promotion
    {
        public int ID { get; set; }
        public string DescEng { get; set; }
        public string DescAR { get; set; }
        public string TitleEng { get; set; }
        public string TitleAR { get; set; }
        public string ImageEng { get; set; }
        public string ImageAR { get; set; }
        public string DescriptionHTML { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> OriginalPrice { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> FK_CreatorID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    }
}
