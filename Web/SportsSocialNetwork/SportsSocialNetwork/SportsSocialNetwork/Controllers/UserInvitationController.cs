using HenchmenWeb.Models.Notifications;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Hubs;
using SportsSocialNetwork.Models.Identity;
using SportsSocialNetwork.Models.Notifications;
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
            if (newsList != null)
            {
                ViewBag.SuggestNews = newsList.FirstOrDefault();
            }

            //load group name
            var _groupService = this.Service<IGroupService>();
            List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f =>
            f.UserId == userId).ToList().Count > 0).ToList();
            if (groupList != null)
            {
                ViewBag.GroupList = groupList;
            }

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
            if (suggestUserList != null)
            {
                ViewBag.suggestUserList = suggestUserList;
            }

            //load follow numbers
            var _followService = this.Service<IFollowService>();
            var _postService = this.Service<IPostService>();
            ViewBag.Following = _followService.GetActive(p => p.FollowerId == curUser.Id).Count();
            ViewBag.Follower = _followService.GetActive(p => p.UserId == curUser.Id).Count();
            ViewBag.PostCount = _postService.GetActive(p => p.UserId == curUser.Id).Count();
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

        public ActionResult CreateInvitation(string[] userId, string sportSelect, string inviteContent, string orderInfo, string groupChatName)
        {
            var result = new AjaxOperationResult<InvitationViewModel>();
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
            Invitation invi = new Invitation();
            invi.SenderId = curUserId;
            invi.InvitationContent = inviteContent + content;
            invi.CreateDate = DateTime.Now;
            invi.Active = true;
            if(groupChatName == null || groupChatName == "")
            {
                groupChatName = "Không tiêu đề";
            }
            invi.Subject = groupChatName;
            invitationService.Create(invi);
            for(int i = 0; i < userId.Length; i++)
            {
                var isDup = false;
                for(int count = 0; count< i; count++)
                {
                    if(userId[i] == userId[count])
                    {
                        isDup = true;
                        break;
                    }
                }
                if (!isDup)
                {
                    UserInvitation UIn = new UserInvitation();
                    Notification noti = new Notification();
                    UIn.InvitationId = invi.Id;
                    UIn.ReceiverId = userId[i];
                    UIn.Active = true;
                    userInvitationService.Create(UIn);
                    noti.InvitationId = UIn.InvitationId;
                    noti.UserId = userId[i];
                    noti.FromUserId = curUserId;
                    noti.CreateDate = DateTime.Now;
                    noti.Active = true;
                    noti.Type = (int)NotificationType.Invitation;
                    noti.Message = curUser.FullName + " đã gửi lời mời bạn cùng chơi thể thao";
                    noti.Title = "Invite";
                    noti.MarkRead = false;
                    notiService.Create(noti);

                    //Fire base noti
                    List<string> registrationIds = GetToken(userId[i]);

                    //registrationIds.Add("dgizAK4sGBs:APA91bGtyQTwOiAgNHE_mIYCZhP0pIqLCUvDzuf29otcT214jdtN2e9D6iUPg3cbYvljKbbRJj5z7uaTLEn1WeUam3cnFqzU1E74AAZ7V82JUlvUbS77mM42xHZJ5DifojXEv3JPNEXQ");

                    NotificationModel amodel = Mapper.Map<NotificationModel>(PrepareNotificationViewModel(noti));

                    if (registrationIds != null && registrationIds.Count != 0)
                    {
                        Android.Notify(registrationIds, null, amodel);
                    }


                    //////////////////////////////////////////////
                    //signalR noti
                    NotificationFullInfoViewModel notiModel = notiService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                    // Get the context for the Pusher hub
                    IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                    // Notify clients in the group
                    hubContext.Clients.User(notiModel.UserId).send(notiModel);
                }
            }
            InvitationViewModel model = Mapper.Map<InvitationViewModel>(invi);
            model.Host = curUser.FullName;
            result.AdditionalData = model;
            result.Succeed = true;
            return Json(result);
        }

        public ActionResult DenyInvitation(int id)
        {
            var result = new AjaxOperationResult();
            var userInviService = this.Service<IUserInvitationService>();
            var userId = User.Identity.GetUserId();
            var uin = userInviService.FirstOrDefaultActive(p => p.InvitationId == id && p.ReceiverId == userId);
            if (uin != null)
            {
                uin.Accepted = false;
                userInviService.Update(uin);
                userInviService.Save();
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
           
            return Json(result);
        }

        public ActionResult AcceptInvitation(int id)
        {
            var result = new AjaxOperationResult();
            var userInviService = this.Service<IUserInvitationService>();
            var userId = User.Identity.GetUserId();
            var uin = userInviService.FirstOrDefaultActive(p => p.InvitationId == id && p.ReceiverId == userId);
            if (uin != null)
            {
                uin.Accepted = true;
                userInviService.Update(uin);
                userInviService.Save();
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        public ActionResult CheckConfirm(int id)
        {
            var result = new AjaxOperationResult<InvitationViewModel>();
            var userInviService = this.Service<IUserInvitationService>();
            var userId = User.Identity.GetUserId();
            var inviService = this.Service<IInvitationService>();
            var invi = inviService.FirstOrDefaultActive(p => p.SenderId == userId && p.Id == id);
            var inviTmp = inviService.FirstOrDefaultActive(p => p.Id == id);
            if (invi != null)
            {
                result.Succeed = true;
            }
            else
            {
                var uin = userInviService.FirstOrDefaultActive(p => p.InvitationId == id && p.ReceiverId == userId);
                if (uin.Accepted == null)
                {
                    InvitationViewModel model = Mapper.Map<InvitationViewModel>(inviTmp);
                    model.Host = inviTmp.AspNetUser.FullName;
                    result.Succeed = false;
                    result.AdditionalData = model;
                }
                else
                {
                    if (uin.Accepted.Value)
                    {
                        result.Succeed = true;
                    }
                    else
                    {
                        InvitationViewModel model = Mapper.Map<InvitationViewModel>(inviTmp);
                        model.Host = inviTmp.AspNetUser.FullName;
                        result.Succeed = false;
                        result.AdditionalData = model;
                    }
                }
            }
            
            return Json(result);
        }

        private List<string> GetToken(String userId)
        {
            var service = this.Service<IFirebaseTokenService>();

            List<FirebaseToken> tokenList = service.Get(x => x.UserId.Equals(userId)).ToList();

            List<string> registrationIds = new List<string>();
            if (tokenList != null)
            {
                foreach (var token in tokenList)
                {
                    registrationIds.Add(token.Token);
                }
            }

            return registrationIds;
        }

        private NotificationCustomViewModel PrepareNotificationViewModel(Notification noti)
        {
            NotificationCustomViewModel result = Mapper.Map<NotificationCustomViewModel>(noti);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.Avatar = noti.AspNetUser1.AvatarImage;

            return result;

        }

    }


}