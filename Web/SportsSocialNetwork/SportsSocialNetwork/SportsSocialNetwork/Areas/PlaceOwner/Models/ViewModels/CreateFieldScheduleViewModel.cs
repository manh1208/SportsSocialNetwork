using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Areas.PlaceOwner.Models.ViewModels
{
    public class CreateFieldScheduleViewModel:FieldScheduleViewModel
    {
        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Sở thích")]
        public string Days { get; set; }

        public string StartDateStr { get; set; }
        public string EndDateStr { get; set; }
        public string StartTimeStr { get; set; }
        public string EndTimeStr { get; set; }

        public int PlaceId { get; set; }
    }
}