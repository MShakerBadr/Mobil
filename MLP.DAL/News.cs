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
    
    public partial class News
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string NewsImage { get; set; }
        public string NewsAbtract { get; set; }
        public string NewsAbtractAr { get; set; }
        public string NewsHTML { get; set; }
        public string NewsHTMLAr { get; set; }
        public Nullable<System.DateTime> NewsDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> FK_CreatorID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    }
}
