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
    
    public partial class Supplier
    {
        public int ID { get; set; }
        public string SuppliersName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string SupplierAddress { get; set; }
        public string ConatctName { get; set; }
        public string SupplierCode { get; set; }
        public string commercialCode { get; set; }
        public string commercialImageName { get; set; }
    }
}
