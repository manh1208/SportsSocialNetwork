using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class NotificationCustomViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int Type { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> InvitationId { get; set; }
        public Nullable<int> OrderId { get; set; }
        public DateTime CreateDate { get; set; }
        public String CreateDateString { get; set; }
        public Nullable<bool> MarkRead { get; set; }
        public bool Active { get; set; }
    }
}