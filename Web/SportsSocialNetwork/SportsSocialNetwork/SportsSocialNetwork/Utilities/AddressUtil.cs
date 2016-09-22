using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Utilities
{
    public class AddressUtil
    {

        private static AddressUtil INSTANCE = null;
        public static String PATH = "~/Content/json/Vietnam.json";

        private AddressUtil()
        {

        }

        public static AddressUtil GetINSTANCE()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new AddressUtil();
            }
            return INSTANCE;
        }

        public Country GetCountry(string path)
        {
            using (StreamReader file = new StreamReader(path))

            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject oo = (JObject)JToken.ReadFrom(reader);

                Country vietnam = oo.ToObject<Country>();
                return vietnam;
            }
           
        }

    }

    public class Country
    {
        public List<Province> VietNamese { get; set; }
    }

    public partial class Ward
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Districtid { get; set; }

        public virtual District District { get; set; }
    }

    public partial class District
    {
       
        public District()
        {
            this.Wards = new HashSet<Ward>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Provinceid { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }

    public partial class Province
    {
        public Province()
        {
            this.Districts = new HashSet<District>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }

}