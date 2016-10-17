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

            ResponseModel<List<GroupMemberViewModel>> response = null;

            try {
                List<GroupMember> memberList = service.GetMemberList(groupId).ToList();

                List<GroupMemberViewModel> result = Mapper.Map<List<GroupMemberViewModel>>(memberList);

                response = new ResponseModel<List<GroupMemberViewModel>>(true, "Danh sách thành viên:", null, result);

            } catch (Exception)
            {
                response = ResponseModel<List<GroupMemberViewModel>>.CreateErrorResponse("Thất bại khi tải danh sách nhóm", systemError);
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult KickMember(int id) {
            var service = this.Service<IGroupMemberService>();

            ResponseModel<GroupMemberViewModel> response = null;

            try {
                GroupMember member = service.KickMember(id);

                if (member.Active == false)
                {
                    GroupMemberViewModel result = Mapper.Map<GroupMemberViewModel>(member);

                    response = new ResponseModel<GroupMemberViewModel>(true, "Đã đuổi thành viên", null, result);
                }
                else {
                    response = ResponseModel<GroupMemberViewModel>.CreateErrorResponse("Đuổi thành viên thất bại", systemError);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<GroupMemberViewModel>.CreateErrorResponse("Đuổi thành viên thất bại", systemError);
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
        public ActionResult SetGroupAdmin(int id, String userId) {
            var service = this.Service<IGroupMemberService>();

            ResponseModel<GroupMemberViewModel> response = null;

            try {
                GroupMember admin = service.SetGroupAdmin(id, userId);

                GroupMemberViewModel result = Mapper.Map<GroupMemberViewModel>(admin);

                response = new ResponseModel<GroupMemberViewModel>(true, "Đã thêm admin", null, result);
            } catch (Exception) {
                response = ResponseModel<GroupMemberViewModel>.CreateErrorResponse("Thêm admin thất bại", systemError);
            }

            return Json(response);
        }
    }
}