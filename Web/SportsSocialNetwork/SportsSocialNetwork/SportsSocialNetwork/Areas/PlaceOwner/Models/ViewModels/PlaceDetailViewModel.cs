using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Areas.PlaceOwner.Models.ViewModels
{
    public class PlaceDetailViewModel : PlaceViewModel
    {
        public AspNetUserViewModel AspNetUser { get; set; }
        public List<PlaceImage> placeImages { get; set; }
        public string AddressString { get; set; }

        public void generateAddress()
        {
            this.AddressString = this.Address;
            this.AddressString += this.Ward != null ? " - Phường " + this.Ward : "";
            this.AddressString += this.District != null ? " - Quận " + this.District : "";
            this.AddressString += this.City != null ? " - " + this.City : "";
        }
    }
}