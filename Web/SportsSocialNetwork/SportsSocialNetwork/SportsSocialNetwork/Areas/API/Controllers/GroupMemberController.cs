using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class GroupMemberController : BaseController
    {
        private String systemError = "An error has occured!";

        [HttpPost]
        public ActionResult JoinGroup(int groupId, String userId) {
            var service = this.Service<IGroupMemberService>();

            ResponseModel<bool> response = null;

            try {
                bool result = service.JoinGroup(groupId,userId);

                response = new ResponseModel<bool>(result,"Bạn đã tham gia nhóm!",null);
            } catch (Exception) {
                response = ResponseModel<bool>.CreateErrorResponse("Tham gia nhóm thất bại!",systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult LeaveGroup(int groupId, String userId)
        {
            var service = this.Service<IGroupMemberService>();

            ResponseModel<bool> response = null;

            try
            {
                bool result = service.LeaveGroup(groupId, userId);

                if (result == false) {
                    response = ResponseModel<bool>.CreateErrorResponse("Thành viên không tồn tại!", systemError);
                }
                else
                {
                    response = new ResponseModel<bool>(result, "Bạn đã rời nhóm!", null);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Rời nhóm thất bại!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ShowMemberList(int groupId) {
            var service = this.Service<IGroupMemberService>();

            ResponseModel<List<GroupMemberDetailViewModel>> response = null;

            try {
                List<GroupMember> memberList = service.GetMemberList(groupId).ToList();

                List<GroupMemberDetailViewModel> result = new List<GroupMemberDetailViewModel>();

                foreach (var member in memberList) {
                    result.Add(PrepareGroupMember(member));
                }

                response = new ResponseModel<List<GroupMemberDetailViewModel>>(true, "Danh sách thành viên:", null, result);

            } catch (Exception)
            {
                response = ResponseModel<List<GroupMemberDetailViewModel>>.CreateErrorResponse("Thất bại khi tải danh sách nhóm", systemError);
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult KickMember(int id) {
            var service = this.Service<IGroupMemberService>();

            ResponseModel<GroupMemberDetailViewModel> response = null;

            try {
                GroupMember member = service.KickMember(id);

                if (member.Active == false)
                {
                    GroupMemberDetailViewModel result = PrepareGroupMember(member);

                    response = new ResponseModel<GroupMemberDetailViewModel>(true, "Đã đuổi thành viên", null, result);
                }
                else {
                    response = ResponseModel<GroupMemberDetailViewModel>.CreateErrorResponse("Đuổi thành viên thất bại", systemError);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<GroupMemberDetailViewModel>.CreateErrorResponse("Đuổi thành viên thất bại", systemError);
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult AddMembers(int groupId, List<String> userIdList) {
            var service = this.Service<IGroupMemberService>();

            ResponseModel<bool> response = null;

            try
            {
                bool result = false;

                foreach (var id in userIdList) {
                    result = service.JoinGroup(groupId, id);
                }

                response = new ResponseModel<bool>(result, "Thành viên đã được thêm vào nhóm!", null);
            }
            catch (Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Thêm thành viên thất bại", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult SetGroupAdmin(int id) {
            var service = this.Service<IGroupMemberService>();

            ResponseModel<GroupMemberDetailViewModel> response = null;

            try {
                GroupMember admin = service.SetGroupAdmin(id);

                GroupMemberDetailViewModel result = PrepareGroupMember(admin);

                response = new ResponseModel<GroupMemberDetailViewModel>(true, "Đã thêm admin", null, result);
            } catch (Exception) {
                response = ResponseModel<GroupMemberDetailViewModel>.CreateErrorResponse("Thêm admin thất bại", systemError);
            }

            return Json(response);
        }

        private GroupMemberDetailViewModel PrepareGroupMember(GroupMember member) {
            var service = this.Service<IAspNetUserService>();

            GroupMemberDetailViewModel result = Mapper.Map<GroupMemberDetailViewModel>(member);

            result.AspNetUser = Mapper.Map<AspNetUserSimpleModel> (service.FindUser(member.UserId));

            return result;
        }
    }

}