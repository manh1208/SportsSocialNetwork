using Microsoft.AspNet.Identity;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
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
    public class UserInvitationController : BaseController
    {
        // GET: UserInvitation
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

        public ActionResult CreateInvitation(string[] userId, string sportSelect, string inviteContent, string orderInfo)
        {
            var result = new AjaxOperationResult();
            var userInvitationService = this.Service<IUserInvitationService>();
            var invitationService = this.Service<IInvitationService>();
            var orderService = this.Service<IOrderService>();
            var userService = this.Service<IAspNetUserService>();
            var curUserId = User.Identity.GetUserId();
            var curUser = userService.FirstOrDefaultActive(p => p.Id == curUserId);
            var notiService = this.Service<INotificationService>();
            string content = "";
            if (orderInfo != null && orderInfo!= "")
            {
                var order = orderService.FirstOrDefaultActive(p => p.OrderCode == orderInfo);
                content = " .Thời gian: " + order.StartTime.ToString("HH:mm") + " - " + order.EndTime.ToString("HH:mm")
                    + " Ngày " + order.StartTime.ToString("dd/MM/yyyy") +". Tại sân: "+order.Field.Name+" ,địa điểm: "
                    +order.Field.Place.Name;
            }
            
            foreach (var id in userId)
            {
                UserInvitation UIn = new UserInvitation();
                Notification noti = new Notification();
                Invitation invi = new Invitation();
                invi.SenderId = curUserId;
                invi.InvitationContent = inviteContent + content;
                invi.CreateDate = DateTime.Now;
                invi.Active = true;
                UIn.Invitation = invi;
                UIn.ReceiverId = id;
                UIn.Active = true;
                userInvitationService.Create(UIn);
                noti.InvitationId = UIn.InvitationId;
                noti.UserId = id;
                noti.FromUserId = curUserId;
                noti.CreateDate = DateTime.Now;
                noti.Active = true;
                noti.Type = (int)NotificationType.Invitation;
                noti.Message = curUser.FullName + " đã gửi lời mời bạn cùng chơi thể thao";
                noti.Title = "Invite";
                noti.MarkRead = false;
                notiService.Create(noti);
            }
            result.Succeed = true;
            return Json(result);
        }
    }

    
}