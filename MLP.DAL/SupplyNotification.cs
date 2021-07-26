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
    
    public partial class SupplyNotification
    {
        public SupplyNotification()
        {
            this.SupplyNotificationActions = new HashSet<SupplyNotificationAction>();
        }
    
        public int ID { get; set; }
        public string NtfMessage { get; set; }
        public string NtfStatus { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string Criticality { get; set; }
        public Nullable<int> FK_UserID { get; set; }
        public Nullable<int> FK_ServiceCenterID { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<SupplyNotificationAction> SupplyNotificationActions { get; set; }
        public virtual ServiceCenter ServiceCenter { get; set; }
    }
}