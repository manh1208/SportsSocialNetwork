using BanleWebsite.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Identity;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
    public class SSNController : BaseController
    {
        public string ViewNameseSymbol { get; private set; }

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
            ViewBag.SuggestNews = newsList.FirstOrDefault();

            //load group name
            var _groupService = this.Service<IGroupService>();
            List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f =>
            f.UserId == userId).ToList().Count > 0).ToList();
            ViewBag.GroupList = groupList;

            //suggest follower
            List<FollowSuggestViewModel> userList = new List<FollowSuggestViewModel>();
            var Coord = new GeoCoordinate();
            bool checkNearBy = false;
            if (curUser.Longitude != null && curUser.Latitude != null)
            {
                Coord = new GeoCoordinate(curUser.Latitude.Value, curUser.Longitude.Value);
                checkNearBy = true;
            }
            var users = _userService.GetActive(p => p.Id != userId && p.AspNetRoles.Where(k => 
            k.Name != "Quản trị viên" && k.Name != "Moderator").ToList().Count > 0 &&
            p.Follows.Where(f => f.Active == true && (f.FollowerId == userId)).ToList().Count == 0).ToList();
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

                if (checkNearBy && (user.Longitude != null && user.Latitude != null))
                {
                    var userCoord = new GeoCoordinate(user.Latitude.Value, user.Longitude.Value);
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
            var _postService = this.Service<IPostService>();
            ViewBag.Following = _followService.GetActive(p => p.FollowerId == curUser.Id).Count();
            ViewBag.Follower = _followService.GetActive(p => p.UserId == curUser.Id).Count();
            ViewBag.PostCount = _postService.GetActive(p => p.UserId == curUser.Id).Count();
            return View();
        }

        public ActionResult GetFollowingUser(int skip, int take)
        {
            var result = new AjaxOperationResult<List<FollowSuggestViewModel>>();
            var userService = this.Service<IAspNetUserService>();
            var userId = User.Identity.GetUserId();
            var userList = userService.GetActive(p => p.Follows.Where(f => f.FollowerId == userId && f.Active).ToList().Count > 0).OrderBy(p => p.FullName).Skip(skip).Take(take).ToList();
            List<FollowSuggestViewModel> suggestUserList = Mapper.Map<List<FollowSuggestViewModel>>(userList);
            result.AdditionalData = suggestUserList;
            result.Succeed = true;
            return Json(result);

        }

        public ActionResult GetOrderBySport(int sportId)
        {
            var result = new AjaxOperationResult<List<OrderSimpleViewModel>>();
            var userId = User.Identity.GetUserId();
            var placeService = this.Service<IPlaceService>();
            var orderService = this.Service<IOrderService>();
            DateTime today = DateTime.Now;
            var orderList = orderService.GetActive(p => p.Field.FieldType.SportId == sportId && p.UserId == userId &&
            p.Status != (int)OrderStatus.Cancel && p.Status != (int)OrderStatus.Unapproved && p.StartTime > today).OrderByDescending(p => p.CreateDate).ToList();
            List<OrderSimpleViewModel> resultList = Mapper.Map<List<OrderSimpleViewModel>>(orderList);
            foreach(var item in resultList)
            {
                var place = placeService.FirstOrDefaultActive(p => p.Fields.Where(f => f.Id == item.FieldId).ToList().Count > 0);
                item.PlaceName = place.Name;
                item.StartTimeString = item.StartTime.ToString("HH:mm");
                item.EndTimeString = item.EndTime.ToString("HH:mm");
                item.PlayDateString = item.StartTime.ToString("dd/MM/yyyy");
            }
            result.AdditionalData = resultList;
            result.Succeed = true;
            return Json(result);
        }

        public ActionResult GetUserBySport(int sportId, int skip, int take)
        {
            var result = new AjaxOperationResult<List<FollowSuggestViewModel>>();
            var userService = this.Service<IAspNetUserService>();
            var userId = User.Identity.GetUserId();
            var curUser = userService.FirstOrDefaultActive(p => p.Id == userId);
            List<FollowSuggestViewModel> userList = new List<FollowSuggestViewModel>();
            var Coord = new GeoCoordinate();
            bool checkNearBy = false;
            if (curUser.Longitude!=null && curUser.Latitude!=null)
            {
                Coord = new GeoCoordinate(curUser.Latitude.Value, curUser.Longitude.Value);
                checkNearBy = true;
            }

            var users = userService.GetActive(p => p.Id != userId && p.AspNetRoles.Where(k =>
            k.Name != "Quản trị viên" && k.Name != "Moderator").ToList().Count > 0 &&
            p.Follows.Where(f => f.Active == true && (f.FollowerId == userId)).ToList().Count == 0 &&
            p.Hobbies.Where(m => m.SportId == sportId).ToList().Count > 0).ToList();
            foreach (var user in users)
            {
                FollowSuggestViewModel model = Mapper.Map<FollowSuggestViewModel>(user);
                if (checkNearBy && (user.Longitude != null && user.Latitude != null))
                {
                    var userCoord = new GeoCoordinate(user.Latitude.Value, user.Longitude.Value);
                    var dis = Coord.GetDistanceTo(userCoord);
                    if (Coord.GetDistanceTo(userCoord) < 5000)
                    {
                        userList.Add(model);
                    }
                }
            }
            List<FollowSuggestViewModel> suggestUserList = userList.OrderBy(p => p.FullName).Skip(skip).Take(take).ToList();
            result.AdditionalData = suggestUserList;
            result.Succeed = true;
            return Json(result);
        }

        public async Task<DataTable> getLocation(string address)
        {
            DataTable dtCoordinates = new DataTable();
            string url = "http://maps.google.com/maps/api/geocode/json?address=" + address + "&sensor=false";
            var googleResults = await loadLocation(url);

            //WebRequest request = WebRequest.Create(url);
            //using (WebResponse response = (HttpWebResponse)request.GetResponse())
            //{
            //    using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            //    {
            //        using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
            //        {
            //            JObject oo = (JObject)JToken.ReadFrom(jsonReader);

            //            //Country vietnam = oo.ToObject<Country>();

            //        }
            //        //DataSet dsResult = new DataSet();
            //        //dsResult.ReadXml(reader);
            //        //dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
            //        //    new DataColumn("Address", typeof(string)),
            //        //    new DataColumn("Latitude",typeof(string)),
            //        //    new DataColumn("Longitude",typeof(string)) });
            //        //foreach (DataRow row in dsResult.Tables["result"].Rows)
            //        //{
            //        //    string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
            //        //    DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
            //        //    dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
            //        //}
            //    }
            //}
            dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(string)),
                        new DataColumn("Address", typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longitude",typeof(string)) });
            System.Diagnostics.Debug.WriteLine("Address: " + address);
            System.Diagnostics.Debug.WriteLine("Result size: " + googleResults.Count);
            foreach (GoogleResult result in googleResults)
            {
               
                dtCoordinates.Rows.Add(result.place_id, 
                                       result.formatted_address, 
                                       result.geometry.location.lat, 
                                       result.geometry.location.lng);
            }


            return dtCoordinates;
        }

        private async Task<List<GoogleResult>> loadLocation(String url)
        {
            List<GoogleResult> results = new List<GoogleResult>();
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync("");
                String json = await response.Content.ReadAsStringAsync();
                JavaScriptSerializer json_serial = new JavaScriptSerializer();
                var googleResult =json_serial.Deserialize<GoogleLocation>(json);
                if (googleResult.status.Equals("OK"))
                {
                    results = googleResult.results;
                }else
                {
                    System.Diagnostics.Debug.WriteLine("Status: " + googleResult.status);
                }
            }
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(url);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          
            
            return results;
        }

        public ActionResult GetSuggestFollow(int pageIndex, int pageSize)
        {
            var userId = User.Identity.GetUserId();
            var _userService = this.Service<IAspNetUserService>();
            List<FollowSuggestViewModel> userList = new List<FollowSuggestViewModel>();
            var curUser = _userService.FirstOrDefaultActive(p => p.Id == userId);
            var Coord = new GeoCoordinate();
            bool checkNearBy = false;
            if (curUser.Longitude != null && curUser.Latitude != null)
            {
                Coord = new GeoCoordinate(curUser.Latitude.Value, curUser.Longitude.Value);
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

                if (checkNearBy && (user.Longitude != null && user.Latitude != null))
                {
                    var userCoord = new GeoCoordinate(user.Latitude.Value, user.Longitude.Value);
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
                user.relationScore = totalOfCommentFromUser + totalOfLikeFromUser + 1;
            }

            List<Post> postList = _postService.GetActive(p => p.UserId == userId || p.ProfileId == userId ||
            p.AspNetUser.Follows.Where(f => f.FollowerId == userId && f.Active && (p.ProfileId == f.UserId || p.ProfileId == null)).ToList().Count > 0
            && p.GroupId == null).ToList();
            
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

        public ActionResult GetSearchUserResult(string keyword, int skip, int take)
        {
            
            var result = new AjaxOperationResult<ShowUserSearchViewModel>();
            var userService = this.Service<IAspNetUserService>();
            //var resultList = new List<AspNetUser>();
            //using (var db = new SSNEntities())
            //{
            //    resultList = userService.FindUserByName(db, keyword, 0, 5).ToList();
            //}
            var followService = this.Service<IFollowService>();
            var userCount = userService.GetActive(p => p.FullName.Contains(keyword) || p.UserName.Contains(keyword) ||
            p.Email.Contains(keyword)).OrderBy(p => p.FullName).ToList().Count;
            var resultList = userService.GetActive(p => p.FullName.Contains(keyword) || p.UserName.Contains(keyword) ||
            p.Email.Contains(keyword)).OrderBy(p => p.FullName).Skip(skip).Take(take).ToList();
            var curUserId = User.Identity.GetUserId();
            var curUser = userService.FirstOrDefaultActive(p => p.Id == curUserId);
            List<FollowSuggestViewModel> searchResultList = new List<FollowSuggestViewModel>();
            //List<FollowSuggestViewModel> searchResultList = Mapper.Map<List<FollowSuggestViewModel>>(ResultList);
            foreach(var user in resultList)
            {
                    FollowSuggestViewModel model = Mapper.Map<FollowSuggestViewModel>(user);
                    var hobbyCount = 1;
                    foreach (var hobby in user.Hobbies.ToList())
                    {
                        foreach (var curHobby in curUser.Hobbies)
                        {
                            if (hobby.SportId == curHobby.SportId)
                            {
                                model.sameSport = hobbyCount;
                                hobbyCount++;
                            }
                        }
                    }
                var tmp = followService.GetActive(p => p.FollowerId == curUserId && p.UserId == user.Id).ToList().Count;
                if(tmp > 0)
                {
                    model.isFollowed = true;
                }else
                {
                    model.isFollowed = false;
                }
                    searchResultList.Add(model);
            }
            ShowUserSearchViewModel searchResult = new ShowUserSearchViewModel();
            searchResult.userCount = userCount;
            searchResult.userList = searchResultList;
            if(searchResultList.Count > 0)
            {
                result.AdditionalData = searchResult;
            }
            
            result.Succeed = true;
            return Json(result);
        }

        public ActionResult GetSearchGroupList(string keyword, int skip, int take)
        {
            var userId = User.Identity.GetUserId();
            VietnameseSymbol VNS = new VietnameseSymbol();
            string kw = VNS.ClearSymbol(keyword);
            var result = new AjaxOperationResult<ShowGroupSearchViewModel>();
            var groupService = this.Service<IGroupService>();
            var groupMemberService = this.Service<IGroupMemberService>();
            var groupCount = groupService.GetActive(p => p.Name.Contains(keyword)).ToList().Count;
            var searchResultList = groupService.GetActive(p => p.Name.Contains(keyword)).OrderBy(p => p.Name).Skip(skip).Take(take).ToList();
            List<GroupSearchViewModel> ResultList = Mapper.Map<List<GroupSearchViewModel>>(searchResultList);
            foreach(var item in ResultList)
            {
                var tmp = groupMemberService.GetActive(p => p.GroupId == item.Id && p.UserId == userId).ToList().Count;
                if(tmp > 0)
                {
                    item.isFollowed = true;
                }else
                {
                    item.isFollowed = false;
                }

                tmp = groupMemberService.GetActive(p => p.GroupId == item.Id && p.UserId == userId && p.Admin).ToList().Count;
                if (tmp > 0)
                {
                    item.isAdmin = true;
                }
                else
                {
                    item.isAdmin = false;
                }
            }
            ShowGroupSearchViewModel searchResult = new ShowGroupSearchViewModel();
            searchResult.groupCount = groupCount;
            searchResult.listGroup = ResultList;
            if (ResultList.Count > 0)
            {
                result.AdditionalData = searchResult;
            }
            result.Succeed = true;
            return Json(result);
        }

        public ActionResult SearchDetail(string keyword)
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
            ViewBag.Keyword = keyword;
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
            ViewBag.SuggestNews = newsList.FirstOrDefault();

            //load group name
            var _groupService = this.Service<IGroupService>();
            List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f =>
            f.UserId == userId).ToList().Count > 0).ToList();
            ViewBag.GroupList = groupList;

            //suggest follower
            List<FollowSuggestViewModel> userList = new List<FollowSuggestViewModel>();
            var Coord = new GeoCoordinate();
            bool checkNearBy = false;
            if (curUser.Longitude != null && curUser.Latitude != null)
            {
                Coord = new GeoCoordinate(curUser.Latitude.Value, curUser.Longitude.Value);
                checkNearBy = true;
            }
            var users = _userService.GetActive(p => p.Id != userId && p.AspNetRoles.Where(k =>
            k.Name != "Quản trị viên" && k.Name != "Moderator").ToList().Count > 0 &&
            p.Follows.Where(f => f.Active == true && (f.FollowerId == userId)).ToList().Count == 0).ToList();
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

                if (checkNearBy && (user.Longitude != null && user.Latitude != null))
                {
                    var userCoord = new GeoCoordinate(user.Latitude.Value, user.Longitude.Value);
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
            var _postService = this.Service<IPostService>();
            ViewBag.Following = _followService.GetActive(p => p.FollowerId == curUser.Id).Count();
            ViewBag.Follower = _followService.GetActive(p => p.UserId == curUser.Id).Count();
            ViewBag.PostCount = _postService.GetActive(p => p.UserId == curUser.Id).Count();
            return View();
        }

    }
}