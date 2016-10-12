using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class AspNetUserOveralViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string AvatarImage { get; set; }
        public string CoverImage { get; set; }
        public List<HobbyViewModel> Hobbies { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public bool Followed { get; set; }
        public int FollowCount { get; set; }
        public int NewsCount { get; set; }
    }
}