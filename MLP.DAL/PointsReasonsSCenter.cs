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
    
    public partial class PointsReasonsSCenter
    {
        public int ID { get; set; }
        public Nullable<int> FK_PointsReasonsID { get; set; }
        public Nullable<int> FK_ServiceCenterID { get; set; }
    
        public virtual PointsReason PointsReason { get; set; }
        public virtual ServiceCenter ServiceCenter { get; set; }
    }
}
