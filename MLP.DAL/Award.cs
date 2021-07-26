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
    
    public partial class Award
    {
        public Award()
        {
            this.RedemptionRequests = new HashSet<RedemptionRequest>();
        }
    
        public int ID { get; set; }
        public Nullable<int> FK_SalesItemID { get; set; }
        public string AwardImage { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<int> FK_LevelID { get; set; }
        public Nullable<int> AwardDuePoints { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> FK_CreatorID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> MaxDiscountValue { get; set; }
        public Nullable<int> MaxRedeemCount { get; set; }
        public int FK_AwardTypeID { get; set; }
    
        public virtual ICollection<RedemptionRequest> RedemptionRequests { get; set; }
        public virtual AwardsType AwardsType { get; set; }
    }
}
