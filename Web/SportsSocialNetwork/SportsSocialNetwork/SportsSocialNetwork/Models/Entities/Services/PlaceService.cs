﻿using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Teek.Models;
using SportsSocialNetwork.Models.Enumerable;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPlaceService
    {

        #region Code from here
        DataTable getLocation(string address);
        void savePlace(Place place);
        IEnumerable<Place> getAllPlace();
        IEnumerable<Place> getPlace(string sport, string province, string district);
        IEnumerable<Place> GetAll(int skip, int take);
        IEnumerable<Place> GetAllOfPlaceOwner(String ownerId);
        Place GetPlaceById(int id);

        Place ChangeStatus(int id, int status);
        IQueryable<Place> GetPlaces(JQueryDataTableParamModel request, out int totalRecord);

        IEnumerable<Place> FindAllPlaceOfPlaceOwner(String userId);

        #endregion

        void test();

    }

    public partial class PlaceService : IPlaceService
    {

        #region Code from here
        public IEnumerable<Place> getAllPlace()
        {
            return this.GetActive(p => p.Approve && (p.Status == (int)PlaceStatus.Active || p.Status == (int)PlaceStatus.Repairing));
        }

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

        public IEnumerable<Place> getPlace(string sport, string province, string district)
        {
            IEnumerable<Place> places = new List<Place>();
            if (sport != null && sport != "")
            {
                var sportId = Int32.Parse(sport);
                if (province != null && province != "")
                {
                    if (district != null && district != "")
                    {
                        places = this.Get(p => p.Approve && (p.Status == (int)PlaceStatus.Active || p.Status == (int)PlaceStatus.Repairing) && p.Fields.Where(f => f.FieldType.SportId == sportId).ToList().Count > 0 && p.City == province && p.District == district).ToList();
                    }
                    else
                    {
                        places = this.Get(p => p.Approve && (p.Status == (int)PlaceStatus.Active || p.Status == (int)PlaceStatus.Repairing) && p.Fields.Where(f => f.FieldType.SportId == sportId).ToList().Count > 0 && p.City == province).ToList();
                    }
                }
                else
                {
                    places = this.Get(p => p.Approve && (p.Status == (int)PlaceStatus.Active || p.Status == (int)PlaceStatus.Repairing) && p.Fields.Where(f => f.FieldType.SportId == sportId).ToList().Count > 0).ToList();
                }
            }
            else
            {
                if (province != null && province != "")
                {
                    if (district != null && district != "")
                    {
                        places = this.Get(p => p.Approve && (p.Status == (int)PlaceStatus.Active || p.Status == (int)PlaceStatus.Repairing) && p.City == province && p.District == district).ToList();
                    }
                    else
                    {
                        places = this.Get(p => p.Approve && (p.Status == (int)PlaceStatus.Active || p.Status == (int)PlaceStatus.Repairing) && p.City == province).ToList();
                    }
                }
                else
                {
                    places = this.getAllPlace();
                }
            }

            return places;
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
            if (searchPlace == null)
            {
                //place.UserId = "8955d736-4fea-45de-96ce-1ebae8265cc8";
                place.Status = (int)PlaceStatus.Pending;
                place.Approve = false;
                place.Active = false;
                place.Latitude = latitude;
                place.Longitude = longtitude;
                place.Avatar = "";

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
                searchPlace.Status = place.Status;
                //searchPlace.Avatar = place.Avatar;
                this.Update(searchPlace);
                this.Save();
            }
        }

        public IEnumerable<Place> GetAll(int skip, int take)
        {
            IEnumerable<Place> placeList;
            placeList = this.GetActive(x=> x.Approve==true).OrderBy(x=> x.Ratings.Count ).Skip(skip).Take(take);
            return placeList;
        }

        public IEnumerable<Place> GetAllOfPlaceOwner(String ownerId) {
            return this.GetActive(x => x.UserId == ownerId);
        }

        public Place GetPlaceById(int id)
        {
            Place place = this.FirstOrDefault(x => x.Id == id);
            if (place != null)
            {
                return place;
            }
            return null;
        }

        public Place ChangeStatus(int id, int status)
        {
            Place place = this.FirstOrDefault(x => x.Id == id);
            this.Get();
            if (place != null)
            {
                place.Status = status;
                this.Save();
                return place;
            }
            return null;
        }

        public IQueryable<Place> GetPlaces(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive();

            var list = list1.Where(
                u => filter == null ||
                u.Name.ToLower().Contains(filter.ToLower()) ||
                u.Name.ToLower().Contains(filter.ToLower()) ||
                u.Email.ToLower().Contains(filter.ToLower())
                );

            //list = list.Where(u => u.AspNetRoles.Where(r => r.Id.Equals(UserRole.Member.ToString())).Count()>0);
            totalRecord = list.Count();
            var result = list.OrderBy(u => u.Name)
                .Skip(request.iDisplayStart)
                             .Take(request.iDisplayLength);

            return result;
        }

        public IEnumerable<Place> FindAllPlaceOfPlaceOwner(String userId) {
            return this.GetActive(x=> x.UserId==userId);
        }

        #endregion

        public void test()
        {

        }
    }
}