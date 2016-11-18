using Microsoft.AspNet.Identity;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.ViewModels;
using SportsSocialNetwork.Utilities;
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
using Teek.Models;

namespace SportsSocialNetwork.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index(string userId)
        {
            var _userService = this.Service<IAspNetUserService>();
            var _sportService = this.Service<ISportService>();
            var _followService = this.Service<IFollowService>();
            var _groupService = this.Service<IGroupService>();
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            var province = vietnam.VietNamese.ToList();
            IEnumerable<SelectListItem> provinceList = province.Select(m => new SelectListItem
            {
                Text = m.Type + " " + m.Name,
                Value = m.Type + " " + m.Name
            }).OrderBy(s => s.Value).ToArray();
            ViewBag.ProvinceList = provinceList;

            AspNetUser user = _userService.FindUser(userId);
            AspNetUserFullInfoViewModel model = Mapper.Map<AspNetUserFullInfoViewModel>(user);
            this.PrepareUserInfo(model);

            //suggest follower
            List<FollowSuggestViewModel> userList = new List<FollowSuggestViewModel>();
            var Coord = new GeoCoordinate();
            bool checkNearBy = false;
            if (user.Longitude != null && user.Latitude != null)
            {
                Coord = new GeoCoordinate(user.Latitude.Value, user.Longitude.Value);
                checkNearBy = true;
            }
            var users = _userService.GetActive(p => p.Id != userId && p.Follows.Where(f => f.Active == true && (f.FollowerId == userId)).ToList().Count == 0).ToList();
            foreach (var item in users)
            {
                FollowSuggestViewModel followsug = Mapper.Map<FollowSuggestViewModel>(item);
                followsug.weight = 0;
                foreach (var follow in item.Follows)
                {
                    if (follow.UserId == user.Id)
                    {
                        followsug.weight += 1;
                        break;
                    }
                }

                if (checkNearBy && (item.Longitude != null && item.Latitude != null))
                {
                    var userCoord = new GeoCoordinate(item.Latitude.Value, item.Longitude.Value);
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
            if (suggestUserList != null)
            {
                ViewBag.suggestUserList = suggestUserList;
            }

            //get sport list for post
            var sports = _sportService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Sport = sports;

            //get all image of user
            ViewBag.userPostImages = this.GetAllPostImageOfUser(userId, 0, 12).ToList();

            //get all people that this user follow
            List<Follow> following = _followService.GetFollowingList(userId).ToList();
            List<AspNetUser> followingUsers = new List<AspNetUser>();
            foreach (var item in following)
            {
                AspNetUser us = _userService.FirstOrDefaultActive(u => u.Id == item.UserId);
                followingUsers.Add(us);
            }
            ViewBag.followingUsers = followingUsers;

            //get list group that this user joined
            List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f =>
           f.UserId == userId && f.Status == (int)GroupMemberStatus.Approved).ToList().Count > 0).ToList();
            if (groupList != null)
            {
                ViewBag.GroupList = groupList;
            }
            IEnumerable<SelectListItem> districtList = new List<SelectListItem>();
            if (model.City != null && model.City != "")
            {
                province = vietnam.VietNamese.Where(p => model.City.Equals(p.Type+" "+p.Name)).ToList();
                if (province != null && province.Count > 0)
                {
                    var district = province.First().Districts.ToList();
                    districtList = district.Select(m => new SelectListItem
                    {
                        Text = m.Type + " " + m.Name,
                        Value = m.Type + " " + m.Name
                    }).OrderBy(s => s.Value).ToArray();
                }
            }
            ViewBag.DistrictList = districtList;

            IEnumerable<SelectListItem> wardList = new List<SelectListItem>();
            if (model.District != null && model.District != "")
            {
                 province = vietnam.VietNamese.Where(p => model.City.Equals(p.Type + " " + p.Name) && p.Districts.Where(f =>
            model.District.Equals(f.Type+" "+f.Name)).ToList().Count > 0).ToList();
                if (province != null && province.Count > 0)
                {
                    var districts = province.First().Districts.Where(p => model.District.Equals(p.Type + " " + p.Name)).ToList();
                    if (districts != null && districts.Count > 0)
                    {
                        var ward = districts.First().Wards.ToList();
                        wardList = ward.Select(m => new SelectListItem
                        {
                            Text = m.Type + " " + m.Name,
                            Value = m.Type + " " + m.Name
                        }).OrderBy(s => s.Value).ToArray();
                    }

                }

            }
            ViewBag.WardList = wardList;
            return View(model);
        }

        public ActionResult GetDistrict(string provinceName)
        {
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            IEnumerable<SelectListItem> districtList = new List<SelectListItem>();
            if (provinceName != null && provinceName != "")
            {
                var province = vietnam.VietNamese.Where(p => provinceName.Equals(p.Type+" "+p.Name)).ToList();
                if (province != null && province.Count > 0)
                {
                    var district = province.First().Districts.ToList();
                    districtList = district.Select(m => new SelectListItem
                    {
                        Text = m.Type + " " + m.Name,
                        Value = m.Type + " " + m.Name
                    }).OrderBy(s => s.Value).ToArray();
                }

            }

            return Json(districtList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWard(string district, string provinceName)
        {
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            IEnumerable<SelectListItem> wardList = new List<SelectListItem>();
            if (district != null && district != "")
            {
                var provinceList = vietnam.VietNamese.Where(p => provinceName.Equals(p.Type + " " + p.Name) && 
                p.Districts.Where(f => district.Equals(f.Type+" "+f.Name)).ToList().Count>0).ToList();
                if (provinceList != null && provinceList.Count > 0)
                {
                    var districtList = provinceList.First().Districts.Where(p => district.Equals(p.Type + " " + p.Name)).ToList();
                    if(districtList !=null && districtList.Count > 0)
                    {
                        var ward = districtList.First().Wards.ToList();
                        wardList = ward.Select(m => new SelectListItem
                        {
                            Text = m.Type + " " + m.Name,
                            Value = m.Type + " " + m.Name
                        }).OrderBy(s => s.Value).ToArray();
                    }
                    
                }

            }

            return Json(wardList, JsonRequestBehavior.AllowGet);
        }
        public IEnumerable<PostImage> GetAllPostImageOfUser(string userId, int skip, int take)
        {
            var _postService = this.Service<IPostService>();
            var _postImageService = this.Service<IPostImageService>();
            List<PostImage> userPostImages = new List<PostImage>();
            List<Post> userPosts = _postService.GetAllPostOfUser(userId).ToList();
            foreach (var item in userPosts)
            {
                List<PostImage> postImages = _postImageService.GetAllPostImage(item.Id).ToList();
                userPostImages.AddRange(postImages);
            }
            return userPostImages.Skip(skip).Take(take);
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

            p.Followed = _followService.CheckFollowOrNot(p.Id, User.Identity.GetUserId());

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

                List<Post> postList = _postService.GetAllProfilePost(userId, skip, take).ToList();
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

        [HttpPost]
        public ActionResult ChangeCoverImage(string userId, HttpPostedFileBase inputCover)
        {
            string containingFolder = "CoverImages";
            var result = new AjaxOperationResult();
            var _userService = this.Service<IAspNetUserService>();

            AspNetUser user = _userService.FirstOrDefaultActive(u => u.Id.Equals(userId));

            if(user != null && inputCover != null)
            {
                FileUploader _fileUploadService = new FileUploader();
                string filePath = _fileUploadService.UploadImage(inputCover, containingFolder);
                user.CoverImage = filePath;
                _userService.UpdateUser(user);
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult ChangeAvatarImage(string userId, HttpPostedFileBase inputAvatar)
        {
            string containingFolder = "AvatarImages";
            var result = new AjaxOperationResult();
            var _userService = this.Service<IAspNetUserService>();

            AspNetUser user = _userService.FirstOrDefaultActive(u => u.Id.Equals(userId));

            if (user != null && inputAvatar != null)
            {
                FileUploader _fileUploadService = new FileUploader();
                string filePath = _fileUploadService.UploadImage(inputAvatar, containingFolder);
                user.AvatarImage = filePath;
                _userService.UpdateUser(user);
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult saveProfile(AspNetUserFullInfoViewModel model, string hobbies)
        {
            var result = new AjaxOperationResult();
            var _userService = this.Service<IAspNetUserService>();

            AspNetUser user = _userService.FindUser(model.Id);
            if(user != null)
            {
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.City = model.City;
                user.Address = model.Address;
                user.District = model.District;
                user.Ward = model.Ward;
                if(user.Ward != null || user.District != null || user.City != null)
                {
                    StringBuilder localtion = new StringBuilder();
                    localtion.Append(user.Address);
                    localtion.Append(" " + user.Ward + " " + user.District + " " + user.City);
                    DataTable coordinate = getLocation(localtion.ToString());
                    double latitude = Double.Parse(coordinate.Rows[0]["Latitude"].ToString());
                    double longtitude = Double.Parse(coordinate.Rows[0]["Longitude"].ToString());
                    user.Latitude = latitude;
                    user.Longitude = longtitude;
                }

                if (_userService.UpdateUser(user) != null)
                {
                    if (!String.IsNullOrEmpty(hobbies))
                    {
                        string[] newHobbies = hobbies.Split(',');
                        if (newHobbies != null)
                        {
                            var _hobbyService = this.Service<IHobbyService>();

                            //find and delete old hobbies
                            List<Hobby> oldHobbies = _hobbyService.GetActive(h => h.UserId.Equals(user.Id)).ToList();
                            foreach (var item in oldHobbies)
                            {
                                _hobbyService.Delete(item);
                            }

                            //add new hobbies
                            foreach (var item in newHobbies)
                            {
                                if (!item.Equals(""))
                                {
                                    Hobby hb = new Hobby();
                                    hb.UserId = user.Id;
                                    hb.SportId = Int32.Parse(item);
                                    _hobbyService.Create(hb);
                                }
                                
                            }
                            result.Succeed = true;
                        }
                        else
                        {
                            result.Succeed = false;
                        }
                    }
                    else
                    {
                        result.Succeed = false;
                    }
                }
                else
                {
                    result.Succeed = false;
                }
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }
    }
}