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
    
    public partial class Bidding
    {
        public int BiddingId { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public Nullable<double> CurrentPrice { get; set; }
        public Nullable<int> UserType { get; set; }
    
        public virtual Cars_Info Cars_Info { get; set; }
        public virtual User User { get; set; }
    }
}
