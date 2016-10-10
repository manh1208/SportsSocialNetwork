using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Teek.Models;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class AspNetUserController : BaseController
    {
        private String systemError = "An error has occured!";

        private String userImagePath = "UserImage\\CuongPK";

        [HttpGet]
        public ActionResult FindUser(String name, int skip, int take) {
            var service = this.Service<IAspNetUserService>();

            ResponseModel<List<AspNetUserOveralViewModel>> response = null;

            try {
                List<AspNetUser> userList = service.FindUserByName(name, skip, take).ToList();

                List<AspNetUserOveralViewModel> result = Mapper.Map<List<AspNetUserOveralViewModel>>(userList);

                response = new ResponseModel<List<AspNetUserOveralViewModel>>(true, "Result list:", null, result);
            } catch (Exception) {
                response = ResponseModel<List<AspNetUserOveralViewModel>>.CreateErrorResponse("Failed to load result!", systemError);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowProfile(String userId, String currentUserId) {
            var service = this.Service<IAspNetUserService>();

            AspNetUser user = null;

            ResponseModel<AspNetUserOveralViewModel> response = null;

            try {
                user = service.FindUser(userId);

                if (user != null)
                {
                    AspNetUserOveralViewModel result = Mapper.Map<AspNetUserOveralViewModel>(user);

                    Follow follow=user.Follows.FirstOrDefault(x=> x.UserId== userId && x.FollowerId==currentUserId && x.Active==true);

                    if (follow != null)
                    {
                        result.Followed = true;
                    }
                    else {
                        result.Followed = false;
                    }

                    response = new ResponseModel<AspNetUserOveralViewModel>(true, "Profile has been successfully loaded!", null, result);
                }
                else
                {
                    response = ResponseModel<AspNetUserOveralViewModel>.CreateErrorResponse("Your profile has failed to load!", systemError);
                }
                
            }
            catch (Exception)
            {
                response = ResponseModel<AspNetUserOveralViewModel>.CreateErrorResponse("Your profile has failed to load!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult UpdateProfile(AspNetUserViewModel model) {
            var service = this.Service<IAspNetUserService>();

            AspNetUser user = null;

            ResponseModel<AspNetUserOveralViewModel> response = null;

            List<String> errorList = new List<string>();

            try {
                user = service.FindUser(model.Id);

                if (user != null) {
                    AspNetUser userInfo = Mapper.Map<AspNetUser>(model);
                    if (ValidateUserInfo(errorList,userInfo))
                    {
                        user = service.UpdateUser(userInfo);

                        AspNetUserOveralViewModel result = Mapper.Map<AspNetUserOveralViewModel>(user);

                        response = new ResponseModel<AspNetUserOveralViewModel>(true, "Your profile has been updated!", null, result);
                    }
                    else {
                        response = new ResponseModel<AspNetUserOveralViewModel>(false, "Your profile has failed to update!", errorList);
                    }

                }
                else {
                    response = ResponseModel<AspNetUserOveralViewModel>.CreateErrorResponse("Your profile has failed to update!", systemError);
                }
            } catch (Exception) {
                response = ResponseModel<AspNetUserOveralViewModel>.CreateErrorResponse("Your profile has failed to update!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ChangeAvatar(String userId, HttpPostedFileBase image) {
            var service = this.Service<IAspNetUserService>();

            ResponseModel<String> response = null;

            FileUploader uploader = new FileUploader();

            try {
                String path = uploader.UploadImage(image, userImagePath);

                String result = service.ChangeAvatar(userId, path);

                response = new ResponseModel<String>(true, "Your Avatar has been changed!", null, result);
            } catch (Exception) {
                response = ResponseModel<String>.CreateErrorResponse("Your Avatar has NOT been changed!",systemError);
            }

            return Json(response);

        }

        [HttpPost]
        public ActionResult ChangeCover(String userId, HttpPostedFileBase image) {
            var service = this.Service<IAspNetUserService>();

            ResponseModel<String> response = null;

            FileUploader uploader = new FileUploader();

            try
            {
                String path = uploader.UploadImage(image, userImagePath);

                String result = service.ChangeCover(userId, path);

                response = new ResponseModel<String>(true, "Your Cover image has been changed!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<String>.CreateErrorResponse("Your Cover image has NOT been changed!", systemError);
            }

            return Json(response);
        }

        private bool ValidateUserInfo(List<String> errorList,AspNetUser userInfo) {
            bool result = true;

            String emailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            String phoneNumberRegex = @"^([0-9]{10,12})$";

            if (!Regex.IsMatch(userInfo.Email,emailRegex)) {
                errorList.Add("Email không hợp lệ!");
                result = false;
            }

            if (userInfo.Birthday>DateTime.Now) {
                errorList.Add("Ngày sinh không hợp lệ");
                result = false;
            }

            if (!Regex.IsMatch(userInfo.PhoneNumber, phoneNumberRegex)) {
                errorList.Add("Số điện thoại không hợp lệ!");
                result = false;
            }

            return result;
        }
    }
}