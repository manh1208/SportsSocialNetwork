using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class GroupSearchViewModel : GroupViewModel
    {
        public SportViewModel sport { get; set; }
        public bool isFollowed { get; set; }

        public bool isAdmin { get; set; }
    }
}