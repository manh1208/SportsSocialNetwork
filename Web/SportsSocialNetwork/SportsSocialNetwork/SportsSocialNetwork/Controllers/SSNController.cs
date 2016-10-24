using Microsoft.AspNet.Identity;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Identity;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
    public class SSNController : BaseController
    {
        public ActionResult Index()
        {
            var _sportService = this.Service<ISportService>();
            var sports = _sportService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Sport = sports;
            var _userService = this.Service<IAspNetUserService>();
            var userId = User.Identity.GetUserId();
            var curUser = _userService.FirstOrDefaultActive(p => p.Id == userId);
            if (curUser == null)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }
            ViewBag.User = curUser;

            //suggest news
            var _newsService = this.Service<INewsService>();
            List<News> newsList = new List<News>();
            foreach (var hobby in curUser.Hobbies)
            {
                List<News> list = _newsService.GetActive(p => p.Category.CategorySports.Where(f =>
                f.SportId == hobby.SportId).ToList().Count > 0).ToList();
                newsList.AddRange(list);
            }

            if (newsList.Count == 0)
            {
                newsList = _newsService.GetActive().ToList();
            }
            ViewBag.SuggestNews = newsList.First();

            //load group name
            var _groupService = this.Service<IGroupService>();
            List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f => 
            f.UserId == userId).ToList().Count > 0).ToList();
            ViewBag.GroupList = groupList;

            //suggest follower
            List<FollowSuggestViewModel> userList = new List<FollowSuggestViewModel>();
            var Coord = new GeoCoordinate();
            bool checkNearBy = false;
            if (curUser.Address != null || curUser.District != null || curUser.Ward != null || curUser.City != null)
            {
                StringBuilder location = new StringBuilder();
                location.Append(curUser.Address);
                location.Append(" " + curUser.Ward + " " + curUser.District + " " + curUser.City);
                DataTable coordinate = getLocation(location.ToString());
                double curUserLatitude = Double.Parse(coordinate.Rows[0]["Latitude"].ToString());
                double curUserLongtitude = Double.Parse(coordinate.Rows[0]["Longitude"].ToString());
                Coord = new GeoCoordinate(curUserLatitude, curUserLongtitude);
                checkNearBy = true;
            }


            var users = _userService.GetActive(p => p.Id != userId && p.Follows.Where(f => f.Active == true && (f.FollowerId == userId)).ToList().Count == 0).ToList();
            foreach (var user in users)
            {
                FollowSuggestViewModel model = Mapper.Map<FollowSuggestViewModel>(user);
                model.weight = 0;
                foreach (var follow in user.Follows)
                {
                    if (follow.UserId == curUser.Id)
                    {
                        model.weight += 1;
                        break;
                    }
                }

                if (checkNearBy && (user.Address != null || user.District != null || user.Ward != null || user.City != null))
                {

                    StringBuilder userLocation = new StringBuilder();
                    userLocation.Append(user.Address);
                    userLocation.Append(" " + user.Ward + " " + user.District + " " + user.City);
                    DataTable userCoordinate = getLocation(userLocation.ToString());
                    double userLatitude = Double.Parse(userCoordinate.Rows[0]["Latitude"].ToString());
                    double userLongtitude = Double.Parse(userCoordinate.Rows[0]["Longitude"].ToString());
                    var userCoord = new GeoCoordinate(userLatitude, userLongtitude);
                    var dis = Coord.GetDistanceTo(userCoord);
                    if (Coord.GetDistanceTo(userCoord) < 5000)
                    {
                        model.weight += 2;
                    }
                }

                int hobbyCount = 1;
                foreach (var hobby in user.Hobbies)
                {
                    foreach (var curHobby in curUser.Hobbies)
                    {
                        if (hobby.SportId == curHobby.SportId)
                        {
                            model.weight = model.weight + hobbyCount * 3;
                            model.sameSport = hobbyCount;
                            hobbyCount++;
                        }
                    }
                }
                userList.Add(model);
            }
            List<FollowSuggestViewModel> suggestUserList = userList.OrderByDescending(p => p.weight).Take(10).ToList();
            ViewBag.suggestUserList = suggestUserList;

            //load follow numbers
            var _followService = this.Service<IFollowService>();
            ViewBag.Following = _followService.GetActive(p => p.FollowerId == curUser.Id).Count();
            ViewBag.Follower = _followService.GetActive(p => p.UserId == curUser.Id).Count();
            return View();
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

        public ActionResult GetSuggestFollow(int pageIndex, int pageSize)
        {
            var userId = User.Identity.GetUserId();
            var _userService = this.Service<IAspNetUserService>();
            List<FollowSuggestViewModel> userList = new List<FollowSuggestViewModel>();
            var curUser = _userService.FirstOrDefaultActive(p => p.Id == userId);
            var Coord = new GeoCoordinate();
            bool checkNearBy = false;
            if (curUser.Address!=null || curUser.District!=null || curUser.Ward!=null || curUser.City != null)
            {
                StringBuilder location = new StringBuilder();
                location.Append(curUser.Address);
                location.Append(" " + curUser.Ward + " " + curUser.District + " " + curUser.City);
                DataTable coordinate = getLocation(location.ToString());
                double curUserLatitude = Double.Parse(coordinate.Rows[0]["Latitude"].ToString());
                double curUserLongtitude = Double.Parse(coordinate.Rows[0]["Longitude"].ToString());
                Coord = new GeoCoordinate(curUserLatitude, curUserLongtitude);
                checkNearBy = true;
            }
            

            var users = _userService.GetActive(p => p.Id != userId && p.Follows.Where(f => f.FollowerId == userId).ToList().Count == 0).ToList();
            foreach (var user in users)
            {
                FollowSuggestViewModel model = Mapper.Map<FollowSuggestViewModel>(user);
                model.weight = 0;
                foreach (var follow in user.Follows)
                {
                    if (follow.UserId == curUser.Id)
                    {
                        model.weight += 1;
                        break;
                    }
                }

                if (checkNearBy && (user.Address != null || user.District != null || user.Ward != null || user.City != null))
                {

                    StringBuilder userLocation = new StringBuilder();
                    userLocation.Append(user.Address);
                    userLocation.Append(" " + user.Ward + " " + user.District + " " + user.City);
                    DataTable userCoordinate = getLocation(userLocation.ToString());
                    double userLatitude = Double.Parse(userCoordinate.Rows[0]["Latitude"].ToString());
                    double userLongtitude = Double.Parse(userCoordinate.Rows[0]["Longitude"].ToString());
                    var userCoord = new GeoCoordinate(userLatitude, userLongtitude);
                    var dis = Coord.GetDistanceTo(userCoord);
                    if (Coord.GetDistanceTo(userCoord) < 5000)
                    {
                        model.weight += 2;
                    }
                }

                int hobbyCount = 1;
                foreach (var hobby in user.Hobbies)
                {
                    foreach (var curHobby in curUser.Hobbies)
                    {
                        if (hobby.SportId == curHobby.SportId)
                        {
                            model.weight = model.weight + hobbyCount*3;
                            model.sameSport = hobbyCount;
                            hobbyCount++;
                        }
                    }
                }
                userList.Add(model);
            }
            var suggestUserList = userList.OrderByDescending(p => p.weight).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return Json(suggestUserList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserProfile()
        {
            return View();
        }
    }
}