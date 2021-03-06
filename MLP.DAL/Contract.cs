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
    
    public partial class Contract
    {
        public Contract()
        {
            this.ContractUsers = new HashSet<ContractUser>();
        }
    
        public int ID { get; set; }
        public string Corporate { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ContractStartDate { get; set; }
        public Nullable<System.DateTime> ContractEndDate { get; set; }
        public Nullable<int> CreatorID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public string CorporateAr { get; set; }
        public string DescriptionAr { get; set; }
        public Nullable<bool> InLoyalty { get; set; }
    
        public virtual ICollection<ContractUser> ContractUsers { get; set; }
    }
}
