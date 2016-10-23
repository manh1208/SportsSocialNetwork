using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class FieldScheduleCreateViewModel
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
    }
}