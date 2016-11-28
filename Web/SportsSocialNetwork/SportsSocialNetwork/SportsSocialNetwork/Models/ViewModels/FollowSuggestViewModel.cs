using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class FollowSuggestViewModel : AspNetUserViewModel
    {
        public int weight { get; set; }
        public int sameSport { get; set; }
        public bool isFollowed { get; set; }
    }
}