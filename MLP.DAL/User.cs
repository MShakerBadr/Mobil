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
    
    public partial class User
    {
        public User()
        {
            this.Customers = new HashSet<Customer>();
            this.Notifications = new HashSet<Notification>();
            this.POSConnectionLogs = new HashSet<POSConnectionLog>();
            this.POSInvoices = new HashSet<POSInvoice>();
            this.SupplyNotifications = new HashSet<SupplyNotification>();
            this.SupplyNotificationActions = new HashSet<SupplyNotificationAction>();
            this.ContractUsers = new HashSet<ContractUser>();
        }
    
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string UserType { get; set; }
        public Nullable<int> FK_ServiceCenterID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> FK_LanguageID { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string Extension { get; set; }
    
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<POSConnectionLog> POSConnectionLogs { get; set; }
        public virtual ICollection<POSInvoice> POSInvoices { get; set; }
        public virtual ICollection<SupplyNotification> SupplyNotifications { get; set; }
        public virtual ICollection<SupplyNotificationAction> SupplyNotificationActions { get; set; }
        public virtual ICollection<ContractUser> ContractUsers { get; set; }
    }
}
