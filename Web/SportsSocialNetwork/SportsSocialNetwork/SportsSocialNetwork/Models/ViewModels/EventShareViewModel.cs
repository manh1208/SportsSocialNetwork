using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class EventShareViewModel : EventViewModel
    {
        public AspNetUserViewModel AspNetUser { get; set; }
    }
}