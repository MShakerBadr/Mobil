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
    
    public partial class MenuSubMenuItem
    {
        public int MenuSubMenuItemID { get; set; }
        public Nullable<int> FK_MenuItemID { get; set; }
        public Nullable<int> FK_SubMenuItemID { get; set; }
    }
}
