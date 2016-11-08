using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public partial class InvitationViewModel
    {
        public string CreateDateString { get; set; }
        public List<UserInvitationViewModel> UserInvitations { get; set; }
        public AspNetUserOveralViewModel Sender { get; set; }
        public int numOfUser { get; set; }
        public string Host { get; set; }
    }
}