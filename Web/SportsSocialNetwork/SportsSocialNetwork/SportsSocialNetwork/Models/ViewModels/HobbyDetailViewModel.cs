using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class HobbyDetailViewModel : HobbyViewModel
    {
        public SportViewModel Sport { get; set; }
    }
}