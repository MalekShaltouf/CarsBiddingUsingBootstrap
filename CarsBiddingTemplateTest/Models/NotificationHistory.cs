//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarsBiddingUsingBootstrap.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NotificationHistory
    {
        public int NotificationId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string EnglishMessage { get; set; }
        public string NativeMessage { get; set; }
        public string MainPhoto { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<bool> NotificationStatus { get; set; }
        public Nullable<int> NotificationType { get; set; }
        public Nullable<int> CarId { get; set; }
    
        public virtual User User { get; set; }
        public virtual Cars_Info Cars_Info { get; set; }
    }
}
