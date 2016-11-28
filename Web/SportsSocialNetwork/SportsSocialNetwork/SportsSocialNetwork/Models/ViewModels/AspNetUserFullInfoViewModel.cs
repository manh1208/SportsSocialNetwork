using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class AspNetUserFullInfoViewModel : AspNetUserViewModel
    {
        public List<HobbyDetailViewModel> Hobbies { get; set; }
        public String BirthdayString { get; set; }
        public bool Followed { get; set; }
        public int FollowCount { get; set; }
        public int FollowedCount { get; set; }
        public int PostCount { get; set; }
        public bool isOwner { get; set; }
    }
}