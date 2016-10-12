using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class FieldDetailViewModel: FieldViewModel
    {
        public String Avatar { get; set; }

        public List<FieldImageViewModel> FieldImages { get; set; }

    }
}