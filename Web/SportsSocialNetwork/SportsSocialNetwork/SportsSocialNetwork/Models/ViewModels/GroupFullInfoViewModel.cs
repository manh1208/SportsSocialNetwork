﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class GroupFullInfoViewModel : GroupViewModel
    {
        public SportViewModel Sport { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsMember { get; set; }
        public int MemberCount { get; set; }
        public int PostCount { get; set; }
        public bool isPendingMember { get; set; }
        public string PlaceName { get; set; }
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
        public string Days { get; set; }
    }
}