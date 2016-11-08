using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class NotificationFullInfoViewModel : NotificationViewModel
    {
        public AspNetUserViewModel AspNetUser { get; set; }
        public AspNetUserViewModel AspNetUser1 { get; set; }
        public GroupViewModel Group { get; set; }
        public InvitationViewModel Invitation { get; set; }
        public OrderViewModel Order { get; set; }
        public PostViewModel Post { get; set; }
        public String CreateDateString { get; set; }

    }
}