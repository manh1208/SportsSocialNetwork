using Microsoft.AspNet.Identity;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
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
                            model.weight = model.weight + hobbyCount * 3;
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

        [HttpPost]
        public ActionResult GetNewFeedPost(string userId, int skip, int take)
        {
            var result = new AjaxOperationResult<IEnumerable<PostGeneralViewModel>>();

            var _postService = this.Service<IPostService>();
            var _postCommentService = this.Service<IPostCommentService>();
            var _userService = this.Service<IAspNetUserService>();
            var _likeService = this.Service<ILikeService>();
            var _commentService = this.Service<IPostCommentService>();
            var _sportService = this.Service<ISportService>();
            //Cal Relation Score
            List<AspNetUser> users = _userService.GetActive(p => p.Follows.Where(f => (f.UserId == userId ||
            f.FollowerId == userId) && f.Active).ToList().Count > 0).ToList();
            List<AspNetUserRelationViewModel> listUser = Mapper.Map<List<AspNetUserRelationViewModel>>(users);
            foreach(var user in listUser)
            {
                var totalOfLikeFromUser = _likeService.GetActive(p => p.UserId == userId && p.Post.UserId == user.Id).ToList().Count;
                var totalOfCommentFromUser = _commentService.GetActive(p => p.UserId == userId && p.Post.UserId == user.Id).ToList().Count;
                user.relationScore = totalOfCommentFromUser + totalOfLikeFromUser;
            }

            List<Post> postList = _postService.GetActive(p => p.UserId == userId ||
            p.AspNetUser.Follows.Where(f => f.FollowerId == userId && f.Active).ToList().Count > 0 ||
            p.Group.GroupMembers.Where(g => g.UserId == userId && g.Active).ToList().Count > 0).ToList();

            
            List<PostGeneralViewModel> listPostVM = Mapper.Map<List<PostGeneralViewModel>>(postList);

            foreach (var item in listPostVM)
            {
                PrepareDetailPostData(item, userId);
            }

            foreach (var post in listPostVM)
            {

                //Cal Relation Score
                float relaScore = 0;
                if(post.UserId == userId)
                {
                    relaScore = 1;
                }
                foreach (var user in listUser)
                {
                    if(user.Id == post.UserId)
                    {
                        relaScore = user.relationScore;
                        break;
                    }
                }

                //Cal PostWeight
                int postWeight = 0;
                List<Sport> sportList = _sportService.GetActive(p => p.Hobbies.Where(f =>
                 f.UserId == userId).ToList().Count > 0).ToList();
                if (sportList != null)
                {
                    foreach(var item in post.PostSports)
                    {
                        foreach (var sport in sportList)
                        {
                            if(item.SportId == sport.Id)
                            {
                                postWeight++;
                            }
                        }
                    }
                    
                }
                post.PostWeight = postWeight;

                //Cal TimeDecay
                post.TimeDecay = _postService.CalculateTimeDecay(post.LatestInteractionTime.Value);
                //Cal PostRank
                post.PostRank = (relaScore * (post.PostWeight + 1)) / post.TimeDecay;


            }

            List<PostGeneralViewModel> listPost = listPostVM.OrderByDescending(p => p.PostRank).Skip(skip).Take(take).ToList();

            result.AdditionalData = listPost;
            result.Succeed = true;
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
            //p.PostAge = _postService.CalculatePostAge(p.EditDate == null ? p.CreateDate : p.EditDate.Value);
            p.PostAge = _postService.CalculatePostAge(p.CreateDate);
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