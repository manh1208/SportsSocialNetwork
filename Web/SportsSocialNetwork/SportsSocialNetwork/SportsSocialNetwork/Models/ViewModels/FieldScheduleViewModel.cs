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
    }
}