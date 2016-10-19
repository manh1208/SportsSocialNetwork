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
        public String UserId { get; set; }
        public String UserName { get; set; }
        public String FullName { get; set; }
        public String PhoneNumber { get; set; }
        public int FieldId { get; set; }
        public String FieldName { get; set; }
        public int PlaceId { get; set; }
        public String PlaceName { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
        public String StatusString { get; set; }
        public int PaidType { get; set; }
        public String PaidTypeString { get; set; }
        public String QRCodeUrl { get; set; }
    }
}