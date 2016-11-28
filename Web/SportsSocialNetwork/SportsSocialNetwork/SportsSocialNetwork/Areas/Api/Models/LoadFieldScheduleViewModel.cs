using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Areas.Api.Models
{
    public class LoadFieldScheduleViewModel: FieldScheduleViewModel
    {
        public string Days { get; set; }

        public string StartDateStr { get; set; }
        public string EndDateStr { get; set; }
        public string StartTimeStr { get; set; }
        public string EndTimeStr { get; set; }

        public int PlaceId { get; set; }
    }
}