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
    
    public partial class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FromUserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int Type { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> InvitationId { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> MarkRead { get; set; }
        public bool Active { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
        public virtual Invitation Invitation { get; set; }
        public virtual Order Order { get; set; }
        public virtual Post Post { get; set; }
    }
}
