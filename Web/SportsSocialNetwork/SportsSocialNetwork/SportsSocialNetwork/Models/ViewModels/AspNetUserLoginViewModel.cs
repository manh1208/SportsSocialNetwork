using SportsSocialNetwork.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class UserLoginViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public String FullName { get; set; }
        public string AvatarImage { get; set; }
        public string CoverImage { get; set; }
        public List<HobbyViewModel> Hobbies { get; set; }
        public List<AspNetRoleViewModel> AspNetRoles { get; set; }
    }
}