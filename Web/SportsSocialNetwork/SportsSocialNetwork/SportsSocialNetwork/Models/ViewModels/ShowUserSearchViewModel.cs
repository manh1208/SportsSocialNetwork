using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class ShowUserSearchViewModel
    {
        public List<FollowSuggestViewModel> userList { get; set; }
        public int userCount { get; set; }
    }
}