using SportsSocialNetwork.Models.Entities.Repositories;
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPlaceService
    {

        IEnumerable<Place> getAllPlace();
    }
    public partial class PlaceService : IPlaceService
    {
        public IEnumerable<Place> getAllPlace()
        {
            return this.GetActive();
        }

    }


        DataTable getLocation(string address);
        void savePlace(Place place);
        //void deletePlace(Place place);
    }
    public partial class PlaceService : IPlaceService
    {
        public DataTable getLocation(string address)
        {
            DataTable dtCoordinates = new DataTable();
            string url = "http://maps.google.com/maps/api/geocode/xml?address=" + address + "&sensor=false";
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    
                    dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                        new DataColumn("Address", typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longitude",typeof(string)) });
                    foreach (DataRow row in dsResult.Tables["result"].Rows)
                    {
                        string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                        DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                        dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                    }
                }
            }
            return dtCoordinates;
        }

        public void savePlace(Place place)
        {
            //get coordinate
            StringBuilder localtion = new StringBuilder();
            localtion.Append(place.Address);
            localtion.Append(" " + place.Ward + " " + place.District + " " + place.City);
            DataTable coordinate = getLocation(localtion.ToString());
            double latitude = Double.Parse(coordinate.Rows[0]["Latitude"].ToString());
            double longtitude = Double.Parse(coordinate.Rows[0]["Longitude"].ToString());

            Place searchPlace = this.FirstOrDefaultActive(p => p.Id == place.Id);
            if(searchPlace == null)
            {
                place.UserId = "1";
                place.Status = 1;
                place.Approve = false;
                place.Active = false;
                place.Latitude = latitude;
                place.Longitude = longtitude;
                this.Create(place);
                this.Save();
            }
            else
            {
                searchPlace.Name = place.Name;
                searchPlace.City = place.City;
                searchPlace.District = place.District;
                searchPlace.Ward = place.Ward;
                searchPlace.Address = place.Address;
                searchPlace.Email = place.Email;
                searchPlace.PhoneNumber = place.PhoneNumber;
                searchPlace.StartTime = place.StartTime;
                searchPlace.EndTime = place.EndTime;
                searchPlace.Description = place.Description;
                searchPlace.Latitude = latitude;
                searchPlace.Longitude = longtitude;
                this.Update(searchPlace);
                this.Save();
            }
        }

        //public void deletePlace(Place place)
        //{
        //    this.Deactivate(place);
        //}
    }
}