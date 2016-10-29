using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class GroupMemberFullInfoModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public AspNetUserFullInfoViewModel AspNetUser { get; set; }
        public bool Admin { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public bool isFollowed { get; set; }
    }
}