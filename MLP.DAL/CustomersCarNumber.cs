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
    
    public partial class CustomersCarNumber
    {
        public int ID { get; set; }
        public string CarNumber { get; set; }
        public string FK_CustomerCode { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string Car_Code { get; set; }
        public string model { get; set; }
        public string year { get; set; }
        public string vendor { get; set; }
        public string Motor { get; set; }
        public Nullable<int> FK_Engine_Type { get; set; }
    
        public virtual EngineType EngineType { get; set; }
    }
}
