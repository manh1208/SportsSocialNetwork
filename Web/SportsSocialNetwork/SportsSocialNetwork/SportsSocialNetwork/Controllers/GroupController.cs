using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.Enumerable;
using Teek.Models;
using Microsoft.AspNet.SignalR;
using SportsSocialNetwork.Models.Hubs;
using SportsSocialNetwork.Models.Identity;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
    public class GroupController : BaseController
    {
        // GET: Group
        public ActionResult Index(int? id)
        {
            var _groupService = this.Service<IGroupService>();
            var _groupMemberService = this.Service<IGroupMemberService>();
            var _postService = this.Service<IPostService>();
            var _sportService = this.Service<ISportService>();
            var _followService = this.Service<IFollowService>();
            var _userService = this.Service<IAspNetUserService>();
            var _challengeService = this.Service<IChallengeService>();
            var _hobbyService = this.Service<IHobbyService>();
            var _placeService = this.Service<IPlaceService>();

            Group group = _groupService.FirstOrDefaultActive(g => g.Id == id);
            if(group != null)
            {
                GroupFullInfoViewModel model = Mapper.Map<GroupFullInfoViewModel>(group);

                //check current user role
                string curUserId = User.Identity.GetUserId();

                GroupMemberRole role = _groupMemberService.CheckRoleMember(curUserId, id.Value);

                switch (role)
                {
                    case GroupMemberRole.Admin:
                        model.IsAdmin = true;
                        model.IsMember = true;
                        model.isPendingMember = false;
                        break;

                    case GroupMemberRole.Member:
                        model.IsAdmin = false;
                        model.IsMember = true;
                        model.isPendingMember = false;
                        break;

                    case GroupMemberRole.NotMember:
                        model.IsAdmin = false;
                        model.IsMember = false;
                        model.isPendingMember = false;
                        break;

                    case GroupMemberRole.PendingMember:
                        model.IsAdmin = false;
                        model.IsMember = false;
                        model.isPendingMember = true;
                        break;
                }

                //List<GroupMember> listGroup = _groupMemberService.GetActive(g => g.UserId.Equals(curUserId)).ToList();
                //foreach (var item in listGroup)
                //{
                //    if(item.GroupId == model.Id)
                //    {
                //        model.IsMember = true;
                //        if(item.Admin)
                //        {
                //            model.IsAdmin = true;
                //        }
                //    }
                //}

                //post count
                model.PostCount = _postService.GetActive(p => p.GroupId == id).ToList().Count();

                //member count
                model.MemberCount = _groupMemberService.GetActive(g => g.GroupId == id && g.Status == (int)GroupMemberStatus.Approved).ToList().Count();

                if (model.PlaceId != null)
                {
                    var place = _placeService.FirstOrDefaultActive(p => p.Id == model.PlaceId);
                    if (place != null)
                    {
                        model.PlaceName = place.Name;
                    }
                }
                if (model.StartTime != null)
                {
                    model.StartTimeString = model.StartTime.Value.Hours.ToString("00") + ":" + model.StartTime.Value.Minutes.ToString("00");
                }
                if (model.EndTime != null)
                {
                    model.EndTimeString = model.EndTime.Value.Hours.ToString("00") + ":" + model.EndTime.Value.Minutes.ToString("00");
                }
                if (model.AvailableDays != null)
                {
                    var bits = new bool[8];
                    for (var i = 7; i >= 0; i--)
                    {
                        bits[i] = (model.AvailableDays & (1 << i)) != 0;
                    }
                    var dayOfWeek = "";
                    if (bits[1])
                    {
                        dayOfWeek += "CN ";
                    }
                    if (bits[2])
                    {
                        dayOfWeek += "T2 ";
                    }
                    if (bits[3])
                    {
                        dayOfWeek += "T3 ";
                    }
                    if (bits[4])
                    {
                        dayOfWeek += "T4 ";
                    }
                    if (bits[5])
                    {
                        dayOfWeek += "T5 ";
                    }
                    if (bits[6])
                    {
                        dayOfWeek += "T6 ";
                    }
                    if (bits[7])
                    {
                        dayOfWeek += "T7 ";
                    }
                    model.Days = dayOfWeek;
                }

                //get sport list for post
                var sports = _sportService.GetActive()
                                .Select(s => new SelectListItem
                                {
                                    Text = s.Name,
                                    Value = s.Id.ToString()
                                }).OrderBy(s => s.Value);
                ViewBag.Sport = sports;

                var friends = new List<SelectListItem>();
                List<GroupMember> gmList = _groupMemberService.GetActive(g => g.GroupId == id.Value).ToList();
                //get followed friend
                friends = _followService.GetActive(f => f.FollowerId == curUserId)
                                .Select(s => new SelectListItem
                                {
                                    Text = s.AspNetUser.FullName,
                                    Value = s.AspNetUser.Id
                                }).OrderBy(s => s.Value).ToList();

                int frCount = friends.Count();
                var tmpFrs = new List<SelectListItem>();
                tmpFrs.AddRange(friends);
                for (int i = 0; i < frCount; i++)
                {
                    SelectListItem si = tmpFrs.ElementAt(i);
                    foreach (var it in gmList)
                    {
                        if(si.Value.Equals(it.UserId) || tmpFrs.ElementAt(i).Equals(curUserId))
                        {
                            friends.Remove(tmpFrs.ElementAt(i));
                        }
                    }
                }

                ViewBag.friends = friends;

                //member
                List<GroupMember> ListGroupMember = _groupMemberService.GetActive(g => g.GroupId == id && g.Status == (int)GroupMemberStatus.Approved).ToList();
                List<GroupMemberFullInfoModel> ListGroupMemberVM = Mapper.Map<List<GroupMemberFullInfoModel>>(ListGroupMember);
                for (int i = 0; i < ListGroupMember.Count(); i++)
                {
                    GroupMember gm = ListGroupMember.ElementAt(i);
                    AspNetUser user = _userService.FirstOrDefaultActive(u => u.Id == gm.UserId);
                    AspNetUserFullInfoViewModel userFull = Mapper.Map<AspNetUserFullInfoViewModel>(user);
                    ListGroupMemberVM[i].AspNetUser = userFull;
                }

                //list pending member
                List<GroupMember> ListPendingMember = _groupMemberService.GetActive(g => g.GroupId == id && g.Status == (int)GroupMemberStatus.Pending).ToList();
                List<GroupMemberFullInfoModel> ListPendingMemberVM = Mapper.Map<List<GroupMemberFullInfoModel>>(ListPendingMember);
                for (int i = 0; i < ListPendingMember.Count(); i++)
                {
                    GroupMember gm = ListPendingMember.ElementAt(i);
                    AspNetUser user = _userService.FirstOrDefaultActive(u => u.Id == gm.UserId);
                    AspNetUserFullInfoViewModel userFull = Mapper.Map<AspNetUserFullInfoViewModel>(user);
                    ListPendingMemberVM[i].AspNetUser = userFull;
                }

                //get list user that cur user followed
                List<Follow> listFollow = _followService.GetActive(f => f.FollowerId == curUserId).ToList();
                if (listFollow != null && listFollow.Count > 0)
                {
                    foreach (var followedUser in listFollow)
                    {
                        foreach (var groupMember in ListGroupMemberVM)
                        {
                            if (followedUser.UserId == groupMember.AspNetUser.Id)
                            {
                                groupMember.isFollowed = true;
                            }
                        }
                    }
                }

                //get list group that this user joined
                List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f =>
                f.UserId == curUserId && f.Status == (int)GroupMemberStatus.Approved).ToList().Count > 0).ToList();
                ViewBag.GroupList = groupList;

                //WWWWWWWWWWWWWWWWWWWWWWWWTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
                //get suggest group
                List<Group> groupListSG = _groupService.GetActive(p => p.GroupMembers.Where(f =>
                f.UserId == curUserId).ToList().Count > 0).ToList();
                ViewBag.GroupList = groupList;
                List<Group> AllGroups = _groupService.GetActive().ToList();
                List<Group> TempGroups = AllGroups.Except(groupListSG).ToList();

                List<Hobby> listHobbies = _hobbyService.GetActive(h => h.UserId.Equals(curUserId)).ToList();
                List<Group> suggestGroups = new List<Group>();
                foreach (var item in TempGroups)
                {
                    foreach (var item1 in listHobbies)
                    {
                        if (item.SportId == item1.SportId)
                        {
                            suggestGroups.Add(item);
                        }
                    }
                }
                List<GroupFullInfoViewModel> suggestGroupsVM = Mapper.Map<List<GroupFullInfoViewModel>>(suggestGroups);

                //List<GroupMember> listGroup = _groupMemberService.GetActive(g => g.UserId.Equals(curUserId) && g.Status == (int)GroupMemberStatus.Approved).ToList();
                //List<Group> suggestGroups = _groupService.GetSuggestGroup(id.Value);
                //List<GroupFullInfoViewModel> suggestGroupsVM = Mapper.Map<List<GroupFullInfoViewModel>>(suggestGroups);
                //foreach (var item in suggestGroupsVM)
                //{
                //    item.MemberCount = _groupMemberService.GetActive(g => g.GroupId == item.Id).ToList().Count();
                //    foreach (var gm in listGroup)
                //    {
                //        if(gm.GroupId == item.Id)
                //        {
                //            item.IsMember = true;
                //        }
                //    }
                //}

                //get suggest group for challenge
                List<Group> suggestGroupChallenge = _groupService.GetActive(g => g.SportId == model.SportId && g.Id != model.Id).ToList();

                //get challenge request
                List<ChallengeDetailViewModel> challengeRequestList = Mapper.Map<List<ChallengeDetailViewModel>>(_challengeService.GetAllChallengeRequest(id.Value).ToList());

                //get list that this group was fighted
                List<ChallengeDetailViewModel> challengedList = Mapper.Map<List<ChallengeDetailViewModel>>(_challengeService.GetChallengedList(id.Value).ToList());

                //get not operate challenge list
                List<ChallengeDetailViewModel> notOperateChallengeList = Mapper.Map<List<ChallengeDetailViewModel>>(_challengeService.GetNotOperateChallengeList(id.Value).ToList());

                //get sent challenge request
                List<ChallengeDetailViewModel> sentChallengeRequest = Mapper.Map<List<ChallengeDetailViewModel>>(_challengeService.GetSentChallengeRequest(id.Value).ToList());

                ViewBag.sentChallengeRequest = sentChallengeRequest;
                ViewBag.notOperateChallengeList = notOperateChallengeList;
                ViewBag.challengedList = challengedList;
                ViewBag.challengeRequestList = challengeRequestList;
                ViewBag.groupMember = ListGroupMemberVM;
                ViewBag.pendingMembers = ListPendingMemberVM;
                ViewBag.groupId = id.Value;
                ViewBag.suggestGroups = suggestGroupsVM;
                ViewBag.suggestGroupChallenge = suggestGroupChallenge;
                return View(model);
            }
            else
            {
                return RedirectToAction("PageNotFound", "Errors");
            }


        }

        public ActionResult UpdateGroup(GroupDetailViewModel model)
        {
            var result = new AjaxOperationResult();
            var groupSerive = this.Service<IGroupService>();
            var groupId = model.Id;
            var group = groupSerive.FirstOrDefaultActive(p => p.Id == groupId);
            if (group != null)
            {
                group.StartTime = model.StartTime;
                group.EndTime = model.EndTime;
                group.PlaceId = model.PlaceId;
                group.Name = model.Name;
                group.Description = model.Description;
                group.SportId = model.SportId;
                if (model.Days != null)
                {
                    string[] days = model.Days.Split(',');
                    int repeatDay = 0;
                    for (int i = 0; i < days.Length; i++)
                    {
                        if (days[i].Length > 0)
                        {
                            int mark = 1 << Int16.Parse(days[i]);
                            repeatDay = repeatDay | mark;
                        }
                    }
                    group.AvailableDays = repeatDay;
                }else
                {
                    group.AvailableDays = null;
                }
                groupSerive.Update(group);
                groupSerive.Save();
                result.Succeed = true;
            }else
            {
                result.Succeed = false;
            }
            return Json(result);
        }
        public ActionResult EditGroup(int id)
        {
            var groupService = this.Service<IGroupService>();
            var _sportService = this.Service<ISportService>();
            var placeService = this.Service<IPlaceService>();
            Group group = groupService.FirstOrDefaultActive(p => p.Id == id);
            GroupDetailViewModel model = Mapper.Map<GroupDetailViewModel>(group);
            var sports = _sportService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Sport = sports;
            var places = placeService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Place = places;
            var dayOfWeek = new List<int>();
            if (model.AvailableDays != null)
            {
                var bits = new bool[8];
                for (var i = 7; i >= 0; i--)
                {
                    bits[i] = (model.AvailableDays & (1 << i)) != 0;
                }
                for (var j = 1; j < bits.Length; j++)
                {
                    if (bits[j])
                    {
                        dayOfWeek.Add(j);
                    }
                }
            }
            ViewBag.availableDayOfWeek = dayOfWeek;
            return this.PartialView(model);
        }
        public ActionResult test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getGroupPost(int groupId, string curUserId, int skip, int take)
        {
            var result = new AjaxOperationResult<IEnumerable<PostGeneralViewModel>>();
            if (groupId != 0)
            {
                var _postService = this.Service<IPostService>();
                var _postCommentService = this.Service<IPostCommentService>();

                List<Post> postList = _postService.GetAllPostsOfGroup(groupId, skip, take).ToList();
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
                if(item.UserId == curUserId)
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
            if(cmtList != null && cmtList.Count > 0)
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
        public ActionResult leaveGroupNotAdmin(string userId, int groupId)
        {
            var _groupMemberService = this.Service<IGroupMemberService>();
            var result = new AjaxOperationResult();

            GroupMember gm = _groupMemberService.FirstOrDefaultActive(p => p.UserId == userId && p.GroupId == groupId);
            if(gm != null)
            {
                gm.Admin = false;
                gm.Status = (int)GroupMemberStatus.Pending;
                _groupMemberService.Update(gm);
                _groupMemberService.Deactivate(gm);
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        public ActionResult showFriendList(int groupId)
        {
            var result = new AjaxOperationResult<IEnumerable<GroupMemberDetailViewModel>>();
            var _groupMemberService = this.Service<IGroupMemberService>();

            List<GroupMember> MemberList = _groupMemberService.GetActive(g => g.GroupId == groupId).ToList();
            if(MemberList != null && MemberList.Count > 0)
            {
                List<GroupMemberDetailViewModel> MemberListVM = Mapper.Map<List<GroupMemberDetailViewModel>>(MemberList);
                result.AdditionalData = MemberListVM;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult MakeGroupAdmin(int groupId, string curAdminId, string desAdminId)
        {
            var _GroupMemberService = this.Service<IGroupMemberService>();
            var _notificationService = this.Service<INotificationService>();
            var result = new AjaxOperationResult();

            GroupMember curAdmin = _GroupMemberService.FirstOrDefaultActive(g => g.GroupId == groupId && g.UserId == curAdminId && g.Admin == true);
            GroupMember desAdmin = _GroupMemberService.FirstOrDefaultActive(g => g.GroupId == groupId && g.UserId == desAdminId && g.Admin == false);

            if(curAdmin != null && desAdmin != null)
            {
                //curAdmin.Admin = false;
                desAdmin.Admin = true;

                //_GroupMemberService.Update(curAdmin);
                _GroupMemberService.Update(desAdmin);
                _GroupMemberService.Save();

                //save noti
                string title = Utils.GetEnumDescription(NotificationType.Other);
                int type = (int)NotificationType.GroupMemberAction;
                string message = curAdmin.AspNetUser.FullName + " đã gán quyền quản trị cho bạn trong nhóm " + curAdmin.Group.Name;

                Notification noti = _notificationService.CreateNoti(desAdminId, curAdminId, title, message, type, null, null, null, curAdmin.Group.Id);

                //////////////////////////////////////////////
                //signalR noti
                NotificationFullInfoViewModel notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                // Get the context for the Pusher hub
                IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                // Notify clients in the group
                hubContext.Clients.User(notiModel.UserId).send(notiModel);



                //Notification noti = new Notification();
                //noti.UserId = desAdminId;
                //noti.FromUserId = curAdminId;
                //noti.Message = curAdmin.AspNetUser.FullName + " đã gán quyền quản trị cho bạn trong nhóm " + curAdmin.Group.Name;
                //noti.MarkRead = false;
                //noti.Type = (int)NotificationType.Other;
                //noti.Title = Utils.GetEnumDescription(NotificationType.Other);
                //noti.CreateDate = DateTime.Now;
                //_notificationService.Create(noti);
                //_notificationService.Save();


                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult RemoveGroupAdmin(int groupId, string curAdminId, string desAdminId)
        {
            var _GroupMemberService = this.Service<IGroupMemberService>();
            var _notificationService = this.Service<INotificationService>();
            var result = new AjaxOperationResult();

            GroupMember curAdmin = _GroupMemberService.FirstOrDefaultActive(g => g.GroupId == groupId && g.UserId == curAdminId && g.Admin == true);
            GroupMember desAdmin = _GroupMemberService.FirstOrDefaultActive(g => g.GroupId == groupId && g.UserId == desAdminId && g.Admin == true);

            if (curAdmin != null && desAdmin != null)
            {
                //curAdmin.Admin = false;
                desAdmin.Admin = false;

                //_GroupMemberService.Update(curAdmin);
                _GroupMemberService.Update(desAdmin);
                _GroupMemberService.Save();


                //save noti
                string title = Utils.GetEnumDescription(NotificationType.Other);
                int type = (int)NotificationType.GroupMemberAction;
                string message = curAdmin.AspNetUser.FullName + " đã bỏ quyền quản trị cho bạn trong nhóm " + curAdmin.Group.Name;

                Notification noti = _notificationService.CreateNoti(desAdminId, curAdminId, title, message, type, null, null, null, curAdmin.Group.Id);

                //////////////////////////////////////////////
                //signalR noti
                NotificationFullInfoViewModel notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                // Get the context for the Pusher hub
                IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                // Notify clients in the group
                hubContext.Clients.User(notiModel.UserId).send(notiModel);

                //Notification noti = new Notification();
                //noti.UserId = desAdminId;
                //noti.FromUserId = curAdminId;
                //noti.Message = curAdmin.AspNetUser.FullName + " đã bỏ quyền quản trị cho bạn trong nhóm " + curAdmin.Group.Name;
                //noti.MarkRead = false;
                //noti.Type = (int)NotificationType.Other;
                //noti.Title = Utils.GetEnumDescription(NotificationType.Other);
                //noti.CreateDate = DateTime.Now;
                //_notificationService.Create(noti);
                //_notificationService.Save();

                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult ApproveMember(string userId)
        {
            var result = new AjaxOperationResult();
            var _groupMemberService = this.Service<IGroupMemberService>();
            var _notificationService = this.Service<INotificationService>();

            GroupMember gm = _groupMemberService.ApproveMember(userId);
            if (gm != null)
            {
                //save noti
                string curUser = User.Identity.GetUserId();
                string title = Utils.GetEnumDescription(NotificationType.Other);
                int type = (int)NotificationType.GroupMemberAction;
                string message = "Bạn đã được chấp nhận làm thành viên trong nhóm " + gm.Group.Name;

                Notification noti = _notificationService.CreateNoti(userId, curUser, title, message, type, null, null, null, gm.Group.Id);


                //////////////////////////////////////////////
                //signalR noti
                NotificationFullInfoViewModel notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                // Get the context for the Pusher hub
                IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                // Notify clients in the group
                hubContext.Clients.User(notiModel.UserId).send(notiModel);

                //Notification noti = new Notification();
                //noti.UserId = userId;
                //noti.Message = "Bạn đã được chấp nhận làm thành viên trong nhóm " + gm.Group.Name;
                //noti.MarkRead = false;
                //noti.Type = (int)NotificationType.Other;
                //noti.Title = Utils.GetEnumDescription(NotificationType.Other);
                //noti.CreateDate = DateTime.Now;
                //_notificationService.Create(noti);
                //_notificationService.Save();

                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult JoinGroup(int groupId, string userId)
        {
            var result = new AjaxOperationResult();
            var _GroupMemberService = this.Service<IGroupMemberService>();
            var _userService = this.Service<IAspNetUserService>();
            var _GroupService = this.Service<IGroupService>();

            if(_GroupMemberService.JoinGroup(groupId, userId))
            {
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        [HttpPost]
        public string CheckIsOnlyOneAdmin(int groupId)
        {
            var _groupMemberService = this.Service<IGroupMemberService>();
            List<GroupMember> listGM = _groupMemberService.GetActive(g => g.GroupId == groupId && g.Admin == true).ToList();
            if(listGM.Count() > 1)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }

        public ActionResult DeleteGroup(int groupId)
        {
            var result = new AjaxOperationResult();
            var _groupService = this.Service<IGroupService>();

            if (_groupService.DeleteGroup(groupId))
            {
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult AddFriendGroup(string AddFriendToGroupList, string userId, int groupId, string admin)
        {
            var result = new AjaxOperationResult();
            var _notificationService = this.Service<INotificationService>();
            bool isAdmin = Convert.ToBoolean(admin);
            if (!String.IsNullOrEmpty(AddFriendToGroupList))
            {
                string[] frd = AddFriendToGroupList.Split(',');
                if (frd != null)
                {
                    var _userService = this.Service<IAspNetUserService>();
                    var _groupService = this.Service<IGroupService>();
                    var _groupMemberService = this.Service<IGroupMemberService>();

                    AspNetUser fromUser = _userService.FindUser(userId);
                    Group group = _groupService.FindGroupById(groupId);

                    foreach (var item in frd)
                    {
                        if (!item.Equals(""))
                        {
                            if(isAdmin)
                            {
                                //add to group
                                if (_groupMemberService.JoinGroupByAdmin(groupId, item))
                                {
                                    //save noti
                                    string title = Utils.GetEnumDescription(NotificationType.GroupInvitation);
                                    int type = (int)NotificationType.GroupInvitation;
                                    string message = fromUser.FullName + " đã mời bạn vào nhóm " + group.Name;

                                    Notification noti = _notificationService.CreateNoti(item, userId, title, message, type, null, null, null, group.Id);


                                    //////////////////////////////////////////////
                                    //signalR noti
                                    NotificationFullInfoViewModel notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                                    // Get the context for the Pusher hub
                                    IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                                    // Notify clients in the group
                                    hubContext.Clients.User(notiModel.UserId).send(notiModel);

                                    //Notification noti = new Notification();
                                    //noti.UserId = item;
                                    //noti.FromUserId = userId;
                                    //noti.Title = Utils.GetEnumDescription(NotificationType.Invitation);
                                    //noti.Type = (int)NotificationType.Invitation;
                                    //noti.Message = fromUser.FullName + " đã mời bạn vào nhóm " + group.Name;
                                    //noti.GroupId = group.Id;
                                    //noti.CreateDate = DateTime.Now;
                                    //noti.MarkRead = false;
                                    //_notificationService.Create(noti);
                                    result.Succeed = true;
                                }
                                else
                                {
                                    result.Succeed = false;
                                }
                            }
                            else
                            {
                                //add to group
                                if (_groupMemberService.JoinGroup(groupId, item))
                                {
                                    //save noti
                                    string title = Utils.GetEnumDescription(NotificationType.GroupInvitation);
                                    int type = (int)NotificationType.GroupInvitation;
                                    string message = fromUser.FullName + " đã mời bạn vào nhóm " + group.Name;

                                    Notification noti = _notificationService.CreateNoti(item, userId, title, message, type, null, null, null, group.Id);


                                    //////////////////////////////////////////////
                                    //signalR noti
                                    NotificationFullInfoViewModel notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                                    // Get the context for the Pusher hub
                                    IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                                    // Notify clients in the group
                                    hubContext.Clients.User(notiModel.UserId).send(notiModel);

                                    //Notification noti = new Notification();
                                    //noti.UserId = item;
                                    //noti.FromUserId = userId;
                                    //noti.Title = Utils.GetEnumDescription(NotificationType.Invitation);
                                    //noti.Type = (int)NotificationType.Invitation;
                                    //noti.Message = fromUser.FullName + " đã mời bạn vào nhóm " + group.Name;
                                    //noti.GroupId = group.Id;
                                    //noti.CreateDate = DateTime.Now;
                                    //noti.MarkRead = false;
                                    //_notificationService.Create(noti);
                                    result.Succeed = true;
                                }
                                else
                                {
                                    result.Succeed = false;
                                }
                            }
                        }
                        else
                        {
                            result.Succeed = false;
                        }
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

        [HttpPost]
        public ActionResult CreateGroup(string GroupCreator, string GroupName, string GroupDescription, string GroupSport)
        {
            var result = new AjaxOperationResult<GroupViewModel>();
            var _groupService = this.Service<IGroupService>();
            var _groupMemberService = this.Service<IGroupMemberService>();
            if(!String.IsNullOrEmpty(GroupCreator) && !String.IsNullOrEmpty(GroupName) && !String.IsNullOrEmpty(GroupDescription) && !String.IsNullOrEmpty(GroupSport))
            {
                int _GroupSport = -1;
                if(int.TryParse(GroupSport, out _GroupSport) == false)
                {
                    result.Succeed = false;
                }
                Group group = new Group();
                group.Name = GroupName;
                group.Description = GroupDescription;
                group.SportId = _GroupSport;
                group.Avatar = "/SSNImages/UserImages/img_default_avatar.png";
                group.CoverImage = "/SSNImages/UserImages/img_default_cover.png";

                if (_groupService.CreateGroup(group) != null)
                {
                    GroupMember gm = new GroupMember();
                    gm.GroupId = group.Id;
                    gm.UserId = GroupCreator;
                    gm.Admin = true;
                    gm.Status = (int)GroupMemberStatus.Approved; //mốt sửa khi có enum group status
                    
                    if(_groupMemberService.CreateGroupMember(gm) != null)
                    {
                        GroupViewModel model = Mapper.Map<GroupViewModel>(group);
                        result.AdditionalData = model;
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
            return Json(result);
        }

        [HttpPost]
        public ActionResult ChangeCoverImage(int groupId, HttpPostedFileBase inputCover)
        {
            string containingFolder = "CoverImages";
            var result = new AjaxOperationResult();
            var _groupService = this.Service<IGroupService>();

            Group group = _groupService.FirstOrDefaultActive(u => u.Id == groupId);

            if (group != null && inputCover != null)
            {
                FileUploader _fileUploadService = new FileUploader();
                string filePath = _fileUploadService.UploadImage(inputCover, containingFolder);
                group.CoverImage = filePath;
                _groupService.UpdateGroup(group);
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult ChangeAvatarImage(int groupId, HttpPostedFileBase inputAvatar)
        {
            string containingFolder = "AvatarImages";
            var result = new AjaxOperationResult();
            var _groupService = this.Service<IGroupService>();

            Group group = _groupService.FirstOrDefaultActive(u => u.Id == groupId);

            if (group != null && inputAvatar != null)
            {
                FileUploader _fileUploadService = new FileUploader();
                string filePath = _fileUploadService.UploadImage(inputAvatar, containingFolder);
                group.Avatar = filePath;
                _groupService.UpdateGroup(group);
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult JoinLeaveGroup(string userId, int groupId)
        {
            var _groupMemberService = this.Service<IGroupMemberService>();
            ResponseModel<bool> response = null;

            try
            {
                int result = _groupMemberService.JoinLeaveGroup(userId, groupId);

                response = new ResponseModel<bool>(true, result.ToString(), null);
            }
            catch (Exception)
            {
                response = new ResponseModel<bool>(false, "fail", null);
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult SendChallengeRequest(int fromGroup, int toGroup, string description)
        {
            var _challengeService = this.Service<IChallengeService>();
            var result = new AjaxOperationResult();
            Challenge cha = _challengeService.CreateChallengeRequest(fromGroup, toGroup, description);
            string curUser = User.Identity.GetUserId();

            if (cha != null)
            {
                var _notificationService = this.Service<INotificationService>();
                var _groupMemberService = this.Service<IGroupMemberService>();

                GroupMember gm = _groupMemberService.FirstOrDefaultActive(g => g.GroupId == toGroup && g.Admin == true && g.Status == (int)GroupMemberStatus.Approved);
                GroupMember fgm = _groupMemberService.FirstOrDefaultActive(g => g.GroupId == fromGroup && g.Admin == true && g.Status == (int)GroupMemberStatus.Approved);
                //save noti
                string title = Utils.GetEnumDescription(NotificationType.GroupChallengeInvitation);
                int type = (int)NotificationType.GroupChallengeInvitation;
                string message = fgm.Group.Name + " đã gửi một lời mời thách đấu cho nhóm " + gm.Group.Name + " của bạn";

                Notification noti = _notificationService.CreateNoti(gm.UserId, curUser, title, message, type, null, null, null, gm.GroupId);

                //////////////////////////////////////////////
                //signalR noti
                NotificationFullInfoViewModel notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                // Get the context for the Pusher hub
                IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                // Notify clients in the group
                hubContext.Clients.User(notiModel.UserId).send(notiModel);


                //Notification noti = new Notification();
                //noti.UserId = gm.UserId;
                ////noti.FromUserId = userId;
                //noti.Title = Utils.GetEnumDescription(NotificationType.Other);
                //noti.Type = (int)NotificationType.Other;
                //noti.Message = fgm.Group.Name + " đã gửi một lời mời thách đấu cho nhóm " + gm.Group.Name;
                //noti.GroupId = gm.Id;
                //noti.CreateDate = DateTime.Now;
                //_notificationService.Create(noti);

                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult UpdateChallenge(int challengeId, int status)
        {
            var _challengeService = this.Service<IChallengeService>();
            var result = new AjaxOperationResult();
            if(_challengeService.UpdateChallenge(challengeId, status) == true)
            {
                Challenge cha = _challengeService.FindById(challengeId);
                ChallengeDetailViewModel chaVM = Mapper.Map<ChallengeDetailViewModel>(cha);
                string curUser = User.Identity.GetUserId();
                //save noti
                var _notificationService = this.Service<INotificationService>();
                var _groupMemberService = this.Service<IGroupMemberService>();
                string title = "";
                int type = -1;
                string message = "";

                switch (status)
                {
                    case (int)ChallengeStatus.NotOperate:
                        GroupMember gm = _groupMemberService.FirstOrDefaultActive(g => g.GroupId == cha.Group1.Id && g.Admin == true && g.Status == (int)GroupMemberStatus.Approved);
                        //save noti
                        title = Utils.GetEnumDescription(NotificationType.GroupChallengeInvitation);
                        type = (int)NotificationType.GroupChallengeInvitation;
                        message = "Nhóm "+ chaVM.Group1.Name +" đã đồng ý lời thách đấu từ nhóm " + cha.Group.Name + " của bạn";
                        Notification noti = _notificationService.CreateNoti(gm.UserId, curUser, title, message, type, null, null, null, cha.Group1.Id);

                        //////////////////////////////////////////////
                        //signalR noti
                        NotificationFullInfoViewModel notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                        // Get the context for the Pusher hub
                        IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                        // Notify clients in the group
                        hubContext.Clients.User(notiModel.UserId).send(notiModel);
                        break;

                    case (int)ChallengeStatus.NotAvailable:
                        GroupMember gm1 = _groupMemberService.FirstOrDefaultActive(g => g.GroupId == cha.Group1.Id && g.Admin == true && g.Status == (int)GroupMemberStatus.Approved);
                        //save noti
                        title = Utils.GetEnumDescription(NotificationType.GroupChallengeInvitation);
                        type = (int)NotificationType.GroupChallengeInvitation;
                        message = "Nhóm " + chaVM.Group1.Name + " đã từ chối lời thách đấu từ nhóm " + cha.Group.Name + " của bạn";
                        Notification noti1 = _notificationService.CreateNoti(gm1.UserId, curUser, title, message, type, null, null, null, cha.Group1.Id);

                        //////////////////////////////////////////////
                        //signalR noti
                        NotificationFullInfoViewModel notiModel1 = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti1));

                        // Get the context for the Pusher hub
                        IHubContext hubContext1 = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                        // Notify clients in the group
                        hubContext1.Clients.User(notiModel1.UserId).send(notiModel1);
                        break;
                }
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult ShowRequestChallengeDetail(int id)
        {
            var result = new AjaxOperationResult<ChallengeDetailViewModel>();
            var _challengeService = this.Service<IChallengeService>();
            Challenge cha = _challengeService.FindById(id);
            if(cha != null)
            {
                ChallengeDetailViewModel model = Mapper.Map<ChallengeDetailViewModel>(cha);
                result.AdditionalData = model;
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