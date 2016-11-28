using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class FollowDetailViewModel : FollowViewModel
    {
        public AspNetUserViewModel User { get; set; }
        public AspNetUserViewModel Follower { get; set; }
    }
}