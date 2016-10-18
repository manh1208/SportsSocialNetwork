using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class OrderSimpleViewModel
    {
        public int Id { get; set; }
        public String CreateDate { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public String UserName { get; set; }
        public String FullName { get; set; }
        public String FieldName { get; set; }
        public String PlaceName { get; set; }
        public String Status { get; set; }
        public String PaidType { get; set; }
        public String QRCodeUrl { get; set; }
    }
}