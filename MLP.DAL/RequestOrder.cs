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
    
    public partial class RequestOrder
    {
        public int ID { get; set; }
        public string FK_RequestNo { get; set; }
        public Nullable<int> FK_CenterID { get; set; }
        public Nullable<int> FK_UserID { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> FK_StatusID { get; set; }
        public Nullable<int> FK_RequestType { get; set; }
        public Nullable<int> FK_ManagerID { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
    }
}