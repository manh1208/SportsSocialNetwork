using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class PlaceOveralViewModel
    {
        public int Id { get; set; }
        public AspNetUserSimpleModel AspNetUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }
        public String StatusString { get; set; }
        public bool Active { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public bool Approve { get; set; }
        public List<SportViewModel> SportList { get; set; }
        public double rate { get; set; }
    }
}