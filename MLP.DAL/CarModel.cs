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
    
    public partial class CarModel
    {
        public int ID { get; set; }
        public string ModeName { get; set; }
        public string ModelNameAR { get; set; }
        public Nullable<int> FK_BrandID { get; set; }
    
        public virtual CarBrand CarBrand { get; set; }
    }
}
