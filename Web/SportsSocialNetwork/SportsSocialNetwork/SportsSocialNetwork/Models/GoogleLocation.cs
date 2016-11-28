using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class GoogleLocation
    {
        public List<GoogleResult> results { get; set; }
        public string status { get; set; }
    }

    public class GoogleResult
    {
        public Geometry geometry { get; set; }
        public string formatted_address { get; set; }
        public string place_id { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}