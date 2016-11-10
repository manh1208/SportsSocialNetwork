using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public partial class NewsViewModel
    {
        public string CreateDateString { get; set; }
        public string CategoryName { get; set; }
        public string Author { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}