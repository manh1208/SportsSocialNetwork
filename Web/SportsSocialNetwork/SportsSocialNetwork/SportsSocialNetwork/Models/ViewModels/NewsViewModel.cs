using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public partial class NewsViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}