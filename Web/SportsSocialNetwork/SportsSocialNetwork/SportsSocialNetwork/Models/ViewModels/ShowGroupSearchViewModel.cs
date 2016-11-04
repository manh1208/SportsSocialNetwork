using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class ShowGroupSearchViewModel
    {
        public List<GroupSearchViewModel> listGroup { get; set; }
        public int groupCount { get; set; }
    }
}