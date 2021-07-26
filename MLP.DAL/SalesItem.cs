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
    
    public partial class SalesItem
    {
        public SalesItem()
        {
            this.BundleItems = new HashSet<BundleItem>();
            this.PointsReasonsProducts = new HashSet<PointsReasonsProduct>();
            this.POSInvoiceItems = new HashSet<POSInvoiceItem>();
            this.RequestItems = new HashSet<RequestItem>();
            this.ServiceCenterInventories = new HashSet<ServiceCenterInventory>();
            this.ServiceCenterSalesItems = new HashSet<ServiceCenterSalesItem>();
            this.SupplyOrderItems = new HashSet<SupplyOrderItem>();
        }
    
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string ItemNameAr { get; set; }
        public Nullable<int> FK_ItemTypeID { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<decimal> ItemPrice { get; set; }
        public Nullable<int> BoxQty { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string BarCode { get; set; }
        public string SerialNumber { get; set; }
        public Nullable<int> FK_CategoryID { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Extention { get; set; }
        public Nullable<int> FK_Category4ID { get; set; }
        public string ItemCode { get; set; }
        public bool IsMobileProduct { get; set; }
        public Nullable<int> FK_MobileCategoryID { get; set; }
        public string DescriptionHTML { get; set; }
        public Nullable<int> NoOfServiceTimeSlots { get; set; }
        public Nullable<int> max { get; set; }
        public Nullable<int> min { get; set; }
        public int LoyaltyPoints { get; set; }
        public Nullable<int> FK_SupplierID { get; set; }
        public Nullable<int> MobileDisplayOrder { get; set; }
    
        public virtual ICollection<BundleItem> BundleItems { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual ICollection<PointsReasonsProduct> PointsReasonsProducts { get; set; }
        public virtual ICollection<POSInvoiceItem> POSInvoiceItems { get; set; }
        public virtual ICollection<RequestItem> RequestItems { get; set; }
        public virtual SalesItemCategory SalesItemCategory { get; set; }
        public virtual ICollection<ServiceCenterInventory> ServiceCenterInventories { get; set; }
        public virtual ICollection<ServiceCenterSalesItem> ServiceCenterSalesItems { get; set; }
        public virtual ICollection<SupplyOrderItem> SupplyOrderItems { get; set; }
        public virtual MobileCategory MobileCategory { get; set; }
    }
}