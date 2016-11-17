using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class OrderShareViewModel :OrderViewModel
    {
        public AspNetUserViewModel AspNetUser { get; set; }
        public FieldDetailViewModel Field { get; set; }
    }
}