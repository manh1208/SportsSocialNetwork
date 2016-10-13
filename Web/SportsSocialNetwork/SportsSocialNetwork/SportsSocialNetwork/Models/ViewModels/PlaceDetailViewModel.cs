using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class PlaceDetailViewModel: PlaceOveralViewModel
    {
        public List<PlaceImageViewModel> PlaceImages { get; set; }
    }
}