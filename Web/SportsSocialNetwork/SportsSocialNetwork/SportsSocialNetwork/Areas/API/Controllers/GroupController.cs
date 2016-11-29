using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teek.Models;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class GroupController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        private String userImagePath = "Group";

        [HttpGet]
        public ActionResult FindGroup(String name, int skip, int take)
        {
            var service = this.Service<IGroupService>();

            List<Group> groupList = null;

            ResponseModel<List<GroupViewModel>> response = null;

            try
            {
                groupList = service.FindGroup(name,skip,take).ToList();

                List<GroupViewModel> result = Mapper.Map<List<GroupViewModel>>(groupList);

                response = new ResponseModel<List<GroupViewModel>>(true, "Kết quả tìm kiếm:", null, result);

            }
            catch (Exception)
            {
                response = ResponseModel<List<GroupViewModel>>.CreateErrorResponse("Không tìm thấy kết quả", systemError);
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult ShowGroupDetail(int id,string currentUser)
        {
            var groupService = this.Service<IGroupService>();

            var groupMemberService = this.Service<IGroupMemberService>();

            ResponseModel<GroupDetailViewModel> response = null;

            try
            {
                Group group = groupService.FindGroupById(id);

                GroupDetailViewModel result = Mapper.Map<GroupDetailViewModel>(group);

                result.IsAdmin = groupMemberService.CheckAdmin(currentUser,result.Id);

                result.IsMember = groupMemberService.CheckMember(currentUser, result.Id);

                response = new ResponseModel<GroupDetailViewModel>(true, "Thông tin nhóm:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<GroupDetailViewModel>.CreateErrorResponse("Tải thông tin thất bại!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult CreateGroup(GroupCreateModel model, String userId) {
            var groupService = this.Service<IGroupService>();

            var groupMemberService = this.Service<IGroupMemberService>();

            ResponseModel<GroupViewModel> response = null;

            try {
                Group group = Mapper.Map<Group>(model);

                if (model.UploadImage != null) {
                    FileUploader uploader = new FileUploader();

                    group.Avatar = uploader.UploadImage(model.UploadImage, userImagePath);
                }

                group = groupService.CreateGroup(group);

                groupMemberService.CreateGroupAdmin(group.Id, userId);

                GroupViewModel result = Mapper.Map<GroupViewModel>(group);

                response = new ResponseModel<GroupViewModel>(true,"Tạo nhóm thành công",null,result);

            } catch (Exception) {
                response = ResponseModel<GroupViewModel>.CreateErrorResponse("Tạo nhóm thất bại!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult EditGroup(Group model) {
            var service = this.Service<IGroupService>();

            ResponseModel<GroupViewModel> response = null;

            try {
                Group group = service.UpdateGroup(model);

                GroupViewModel result = Mapper.Map<GroupViewModel>(group);

                response = new ResponseModel<GroupViewModel>(true, "Thông tin nhóm đã được cập nhật!",null, result);
            } catch (Exception) {
                response = ResponseModel<GroupViewModel>.CreateErrorResponse("Thất bại khi cập nhật thông tin nhóm!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteGroup(int id)
        {
            var service = this.Service<IGroupService>();

            ResponseModel<bool> response = null;

            try {
                Group group = service.FirstOrDefaultActive(x => x.Id == id);

                service.Deactivate(group);

                response = new ResponseModel<bool>(true, "Xóa nhóm thành công", null);

            } catch (Exception) {
                response = ResponseModel<bool>.CreateErrorResponse("Xóa nhóm thất bại", systemError);
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult ChangeCoverImage(int groupId, HttpPostedFileBase image) {
            var service = this.Service<IGroupService>();

            ResponseModel<String> response = null;

            FileUploader uploader = new FileUploader();

            try
            {
                String path = null;

                if (image != null)
                {
                    path = uploader.UploadImage(image, userImagePath);

                }

                String result = service.ChangeCoverImage(groupId, path);

                response = new ResponseModel<String>(true, "Ảnh nhóm đã được cập nhật!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<String>.CreateErrorResponse("Ảnh nhóm chưa được cập nhật!", systemError);
            }

            return Json(response);

        }

        [HttpPost]
        public ActionResult ChangeGroupAvatar(int groupId, HttpPostedFileBase image) {
            var service = this.Service<IGroupService>();

            ResponseModel<String> response = null;

            FileUploader uploader = new FileUploader();

            try
            {
                String path = null;

                if (image != null)
                {
                    path = uploader.UploadImage(image, userImagePath);

                }

                String result = service.ChangeAvatarImage(groupId, path);

                response = new ResponseModel<String>(true, "Ảnh nhóm đã được cập nhật!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<String>.CreateErrorResponse("Ảnh nhóm chưa được cập nhật!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ShowAllJoinedGroup(String userId) {
            var groupService = this.Service<IGroupService>();

            var groupMemberService = this.Service<IGroupMemberService>();

            ResponseModel<List<GroupViewModel>> response = null;

            try {
                List<GroupMember> joinedList = groupMemberService.GetJoinedList(userId).ToList();

                List<Group> groupList = new List<Group>();

                foreach (var j in joinedList) {
                    Group group = groupService.FindGroupById(j.GroupId);
                    if (group != null) {
                        groupList.Add(group);
                    }
                }

                List<GroupViewModel> result = Mapper.Map<List<GroupViewModel>>(groupList);

                response = new ResponseModel<List<GroupViewModel>>(true, "Danh sách nhóm:", null, result);

            } catch (Exception) {
                response = ResponseModel<List<GroupViewModel>>.CreateErrorResponse("Không thể tải danh sách nhóm",systemError);
            }

            return Json(response);
        }

    }
}