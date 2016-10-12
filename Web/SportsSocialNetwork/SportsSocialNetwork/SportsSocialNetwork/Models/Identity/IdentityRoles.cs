using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Identity
{
    public enum IdentityRoles
    {
        Admin,
        NormalUser,
        Organizer
    }
    public static class IdentityMultipleRoles
    {
      
        public const string Admin = "Quản trị viên";
        public const string PlaceOwner = "Chủ sân";
        public const string Moderator = "Moderator";
        public const string SSN = "Thành viên,Chủ sân";
        public const string Member = "Thành viên";
    }
}