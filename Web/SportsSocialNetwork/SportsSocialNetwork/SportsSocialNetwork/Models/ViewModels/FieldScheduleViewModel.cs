using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public partial class FieldScheduleViewModel
    {
        public string FieldName { get; set; }
        public String TypeString { get; set; }
        public String StartTimeString { get; set; }
        public String EndTimeString { get; set; }
        public String StartDateString { get; set; }
        public String EndDateString { get; set; }
        public string PlaceName { get; set; }
        public string availableDayOfWeek { get; set; }
        public int PlaceId { get; set; }
        public List<int> DayOfWeek { get; set; }
    }
}