using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class OrderDetailViewModel : OrderViewModel
    {
        public String CreateDateString { get; set; }

        public String StartTimeString { get; set; }

        public String EndTimeString { get; set; }

        public void CreateDateStrings() {
            this.CreateDateString = this.CreateDate.ToString();
            this.StartTimeString = this.StartTime.ToString();
            this.EndTimeString = this.EndTime.ToString();
        }

    }
}