using Microsoft.AspNet.Identity;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
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
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index(string userId)
        {
            var _userService = this.Service<IAspNetUserService>();
            var _sportService = this.Service<ISportService>();

            AspNetUser user = _userService.FindUser(userId);
            AspNetUserFullInfoViewModel model = Mapper.Map<AspNetUserFullInfoViewModel>(user);
            this.PrepareUserInfo(model);

            //suggest follower
            //suggest follower
            List<FollowSuggestViewModel> userList = new List<FollowSuggestViewModel>();
            var Coord = new GeoCoordinate();
            bool checkNearBy = false;
            if (user.Address != null || user.District != null || user.Ward != null || user.City != null)
            {
                StringBuilder location = new StringBuilder();
                location.Append(user.Address);
                location.Append(" " + user.Ward + " " + user.District + " " + user.City);
                DataTable coordinate = getLocation(location.ToString());
                double curUserLatitude = Double.Parse(coordinate.Rows[0]["Latitude"].ToString());
                double curUserLongtitude = Double.Parse(coordinate.Rows[0]["Longitude"].ToString());
                Coord = new GeoCoordinate(curUserLatitude, curUserLongtitude);
                checkNearBy = true;
            }


            var users = _userService.GetActive(p => p.Id != userId && p.Follows.Where(f => f.Active == true && (f.FollowerId == userId)).ToList().Count == 0).ToList();
            foreach (var item in users)
            {
                FollowSuggestViewModel followsug = Mapper.Map<FollowSuggestViewModel>(user);
                followsug.weight = 0;
                foreach (var follow in item.Follows)
                {
                    if (follow.UserId == user.Id)
                    {
                        followsug.weight += 1;
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
                        followsug.weight += 2;
                    }
                }

                int hobbyCount = 1;
                foreach (var hobby in user.Hobbies)
                {
                    foreach (var curHobby in user.Hobbies)
                    {
                        if (hobby.SportId == curHobby.SportId)
                        {
                            followsug.weight = followsug.weight + hobbyCount * 3;
                            followsug.sameSport = hobbyCount;
                            hobbyCount++;
                        }
                    }
                }
                userList.Add(followsug);
            }
            List<FollowSuggestViewModel> suggestUserList = userList.OrderByDescending(p => p.weight).Take(10).ToList();
            ViewBag.suggestUserList = suggestUserList;

            //get sport list for post
            var sports = _sportService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Sport = sports;

            return View(model);
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

        public void PrepareUserInfo(AspNetUserFullInfoViewModel p)
        {
            var _followService = this.Service<IFollowService>();
            var _postService = this.Service<IPostService>();

            //number that this person following
            p.FollowCount = _followService.GetFollowingCount(p.Id);
            //number of people that follow this person
            p.FollowedCount = _followService.GetFollowerCount(p.Id);

            p.Followed = _followService.FollowUnfollowUser(p.Id, User.Identity.GetUserId());

            if (User.Identity.GetUserId().Equals(p.Id))
            {
                p.isOwner = true;
            }
            else
            {
                p.isOwner = false;
            }

            p.PostCount = _postService.GetPostCountOfUser(p.Id);
        }

        [HttpPost]
        public ActionResult getProfilePost(string userId, string curUserId, int skip, int take)
        {
            var result = new AjaxOperationResult<IEnumerable<PostGeneralViewModel>>();
            if (!String.IsNullOrEmpty(userId))
            {
                var _postService = this.Service<IPostService>();
                var _postCommentService = this.Service<IPostCommentService>();

                List<Post> postList = _postService.GetAllPostOfUser(userId, skip, take).ToList();
                List<PostGeneralViewModel> listPostVM = Mapper.Map<List<PostGeneralViewModel>>(postList);

                foreach (var item in listPostVM)
                {
                    PrepareDetailPostData(item, curUserId);
                }

                result.AdditionalData = listPostVM;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        public void PrepareDetailPostData(PostGeneralViewModel p, string curUserId)
        {
            var _postService = this.Service<IPostService>();
            var _likeService = this.Service<ILikeService>();
            var _postCommentService = this.Service<IPostCommentService>();
            var _postSportService = this.Service<IPostSportService>();

            //like
            List<Like> likeList = _likeService.GetLikeListByPostId(p.Id).ToList();
            p.LikeCount = likeList.Count();
            foreach (var item in likeList)
            {
                if (item.UserId == curUserId)
                {
                    p.Liked = true;
                }
                else
                {
                    p.Liked = false;
                }
            }

            //comment
            List<PostComment> postCmtList = _postCommentService.GetCommentListByPostId(p.Id, 0, 3).ToList();
            p.PostAge = _postService.CalculatePostAge(p.EditDate == null ? p.CreateDate : p.EditDate.Value);
            p.PostComments = Mapper.Map<List<PostCommentDetailViewModel>>(postCmtList);
            p.CommentCount = _postCommentService.GetActive(c => c.PostId == p.Id).ToList().Count();
            foreach (var item in p.PostComments)
            {
                //DateTime dt = DateTime.ParseExact(item.CreateDateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                item.CommentAge = _postCommentService.CalculateCommentAge(item.CreateDate);
            }

            //sport
            List<PostSport> postSportList = _postSportService.GetActive(s => s.PostId == p.Id).ToList();
            p.PostSports = Mapper.Map<List<PostSportDetailViewModel>>(postSportList);
        }

        [HttpPost]
        public ActionResult getMoreCmtByPostId(int postId, int skip, int take)
        {
            var result = new AjaxOperationResult<IEnumerable<PostCommentDetailViewModel>>();
            var _postCommentService = this.Service<IPostCommentService>();
            List<PostComment> cmtList = _postCommentService.GetCommentListByPostId(postId, skip, take).ToList();
            if (cmtList != null && cmtList.Count > 0)
            {
                List<PostCommentDetailViewModel> cmtListVM = Mapper.Map<List<PostCommentDetailViewModel>>(cmtList);
                foreach (var item in cmtListVM)
                {
                    //DateTime dt = DateTime.ParseExact(item.CreateDateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    item.CommentAge = _postCommentService.CalculateCommentAge(item.CreateDate);
                }
                result.AdditionalData = cmtListVM;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }


            return Json(result);
        }
    }
}