using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class EventOveralViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public String StartDateString { get; set; }
        public DateTime EndDate { get; set; }
        public String EndDateString { get; set; }
        public int PlaceId { get; set; }   
        public string PlaceName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public bool Joined { get; set; }
    }
}