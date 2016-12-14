using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class FieldSportViewModel
    {
        public int FieldId { get; set; }

        public string FieldName { get; set; }

        public string Sport { get; set; }
        public string FieldType { get; set; }
    }
}