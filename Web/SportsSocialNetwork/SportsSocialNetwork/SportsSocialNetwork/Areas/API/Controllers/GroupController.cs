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
using Teek.Models;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class GroupController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        private String userImagePath = "UserImage\\CuongPK";

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

                response = new ResponseModel<List<GroupViewModel>>(true, "Results loaded!", null, result);

            }
            catch (Exception)
            {
                response = ResponseModel<List<GroupViewModel>>.CreateErrorResponse("Group list failed to load!", systemError);
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult ShowGroupDetail(int id,String currentUser)
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

                response = new ResponseModel<GroupDetailViewModel>(true, "Group detail loaded!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<GroupDetailViewModel>.CreateErrorResponse("Group detail failed to load!", systemError);
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

                    group.CoverImage = uploader.UploadImage(model.UploadImage, userImagePath);
                }

                group = groupService.CreateGroup(group);

                groupMemberService.CreateGroupAdmin(group.Id, userId);

                GroupViewModel result = Mapper.Map<GroupViewModel>(group);

                response = new ResponseModel<GroupViewModel>(true,"Group Created",null,result);

            } catch (Exception) {
                response = ResponseModel<GroupViewModel>.CreateErrorResponse("Failed to to create group!", systemError);
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

                response = new ResponseModel<GroupViewModel>(true, "Group Information has been edited!",null, result);
            } catch (Exception) {
                response = ResponseModel<GroupViewModel>.CreateErrorResponse("Failed to to edit group information!", systemError);
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
                String path = uploader.UploadImage(image, userImagePath);

                String result = service.ChangeCoverImage(groupId, path);

                response = new ResponseModel<String>(true, "Cover image has been changed!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<String>.CreateErrorResponse("Cover image has NOT been changed!", systemError);
            }

            return Json(response);

        }


    }
}