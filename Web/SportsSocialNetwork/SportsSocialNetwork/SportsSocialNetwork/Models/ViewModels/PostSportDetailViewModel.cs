using SportsSocialNetwork.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class PostSportDetailViewModel : PostSportViewModel
    {
        public SportViewModel Sport { get; set; }
    }
}