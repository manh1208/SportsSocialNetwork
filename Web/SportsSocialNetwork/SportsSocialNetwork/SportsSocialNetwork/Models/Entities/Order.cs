//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportsSocialNetwork.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FieldId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
        public Nullable<int> PaidType { get; set; }
    
        public virtual Field Field { get; set; }
        public virtual User User { get; set; }
    }
}
