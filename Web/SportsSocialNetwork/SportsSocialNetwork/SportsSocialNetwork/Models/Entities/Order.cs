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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Notifications = new HashSet<Notification>();
            this.Posts = new HashSet<Post>();
        }
    
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FieldId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
        public Nullable<int> PaidType { get; set; }
        public string OrderCode { get; set; }
        public Nullable<System.DateTime> TransactionTime { get; set; }
        public string PayerName { get; set; }
        public string PayerPhone { get; set; }
        public string PayerEmail { get; set; }
        public string QRCodeUrl { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Field Field { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
