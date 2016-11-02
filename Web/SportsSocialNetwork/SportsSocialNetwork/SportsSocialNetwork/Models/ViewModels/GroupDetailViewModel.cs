using SportsSocialNetwork.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class GroupDetailViewModel: GroupViewModel
    {
        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }

        public SportViewModel Sport { get; set; }
    }
}