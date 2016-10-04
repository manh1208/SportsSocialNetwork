using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Areas.Admin.Models
{
    public class PlaceDetailViewModel: PlaceViewModel
    {
        public string AddressString { get; set; }
        public AspNetUserViewModel AspNetUser { get; set; }

        public void CreateAddressString()
        {
            this.AddressString = this.Address;
            this.AddressString += this.Ward != null ? " - " + this.Ward : "";
            this.AddressString += this.District != null ? " - " + this.District : "";
            this.AddressString += this.City != null ? " - " + this.City : "";
        }

    }
}