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

namespace SportsSocialNetwork.Controllers
{
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

            GroupFullInfoViewModel model = Mapper.Map<GroupFullInfoViewModel>(_groupService.FirstOrDefaultActive(g => g.Id == id));

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
            model.MemberCount = _groupMemberService.GetActive(g => g.GroupId == id).ToList().Count();
            
            //get sport list for post
            var sports = _sportService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Sport = sports;

            //get followed friend
            var friends = _followService.GetActive(f => f.FollowerId == curUserId)
                            .Select(s => new SelectListItem
                            {
                                Text = s.AspNetUser.FullName,
                                Value = s.AspNetUser.Id
                            }).OrderBy(s => s.Value);
            ViewBag.friends = friends;

            //member
            List<GroupMember> ListGroupMember = _groupMemberService.GetActive(g => g.GroupId == id).ToList();
            List<GroupMemberFullInfoModel> ListGroupMemberVM = Mapper.Map<List<GroupMemberFullInfoModel>>(ListGroupMember);
            for(int i = 0; i < ListGroupMember.Count(); i++)
            {
                GroupMember gm = ListGroupMember.ElementAt(i);
                AspNetUser user = _userService.FirstOrDefaultActive(u => u.Id == gm.UserId);
                AspNetUserFullInfoViewModel userFull = Mapper.Map<AspNetUserFullInfoViewModel>(user);
                ListGroupMemberVM[i].AspNetUser = userFull;
            }
            //get list user that cur user followed
            List<Follow> listFollow = _followService.GetActive(f => f.FollowerId == curUserId).ToList();
            if(listFollow != null && listFollow.Count > 0)
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

            //WWWWWWWWWWWWWWWWWWWWWWWWTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
            //get suggest group
            List<GroupMember> listGroup = _groupMemberService.GetActive(g => g.UserId.Equals(curUserId)).ToList();
            List<Group> suggestGroups = _groupService.GetSuggestGroup(id.Value);
            List<GroupFullInfoViewModel> suggestGroupsVM = Mapper.Map<List<GroupFullInfoViewModel>>(suggestGroups);
            foreach (var item in suggestGroupsVM)
            {
                item.MemberCount = _groupMemberService.GetActive(g => g.GroupId == item.Id).ToList().Count();
                foreach (var gm in listGroup)
                {
                    if(gm.GroupId == item.Id)
                    {
                        item.IsMember = true;
                    }
                }
            }

            //get list group that this user joined
            List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f =>
            f.UserId == curUserId).ToList().Count > 0).ToList();
            ViewBag.GroupList = groupList;

            ViewBag.groupMember = ListGroupMemberVM;
            ViewBag.groupId = id.Value;
            ViewBag.suggestGroups = suggestGroupsVM;
            return View(model);
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
        public ActionResult AddFriendGroup(string AddFriendToGroupList, string userId, int groupId)
        {
            var result = new AjaxOperationResult();
            var _notificationService = this.Service<INotificationService>();

            if (!String.IsNullOrEmpty(AddFriendToGroupList))
            {
                string[] frd = AddFriendToGroupList.Split(',');
                if (frd != null)
                {
                    var _userService = this.Service<IAspNetUserService>();
                    var _groupService = this.Service<IGroupService>();

                    AspNetUser fromUser = _userService.FindUser(userId);
                    Group group = _groupService.FindGroupById(groupId);

                    foreach (var item in frd)
                    {
                        if (!item.Equals(""))
                        {
                            Notification noti = new Notification();
                            noti.UserId = item;
                            noti.FromUserId = userId;
                            noti.Title = Utils.GetEnumDescription(NotificationType.Invitation);
                            noti.Type = (int)NotificationType.Invitation;
                            noti.Message = fromUser.FullName + " đã mời bạn vào nhóm " + group.Name;
                            noti.CreateDate = DateTime.Now;
                            _notificationService.Create(noti);
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
                    gm.Status = 1; //mốt sửa khi có enum group status
                    
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
    }
}