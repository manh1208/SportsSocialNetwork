using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Teek.Models;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class AspNetUserController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        private String userImagePath = "AspNetUser";

        private String AdminRoleId = UserRole.Admin.ToString("d");

        private String ModeratorRoleId = UserRole.Moderator.ToString("d");

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AspNetUserController() { }

        public AspNetUserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        [HttpGet]
        public ActionResult FindUser(String query, int skip, int take)
        {
            var service = this.Service<IAspNetUserService>();

            ResponseModel<List<AspNetUserOveralViewModel>> response = null;

            try
            {
                List<AspNetUser> userList = service.FindUserByName(query, skip, take).ToList();

                userList = FilterMember(userList);

                List<AspNetUserOveralViewModel> result = new List<AspNetUserOveralViewModel>();

                foreach (var user in userList)
                {
                    result.Add(PrepareAspNetUserOveralViewModel(user));
                }

                response = new ResponseModel<List<AspNetUserOveralViewModel>>(true, "Kết quả tìm kiếm:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<AspNetUserOveralViewModel>>.CreateErrorResponse("Tìm kiếm thất bại!", systemError);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowProfile(String userId, String currentUserId)
        {
            var service = this.Service<IAspNetUserService>();

            var followService = this.Service<IFollowService>();

            ResponseModel<AspNetUserOveralViewModel> response = null;

            try
            {
                AspNetUser user = service.FindUser(userId);

                if (user != null)
                {
                    AspNetUserOveralViewModel result = PrepareAspNetUserOveralViewModel(user);

                    Follow follow = user.Follows.FirstOrDefault(x => x.UserId == userId && x.FollowerId == currentUserId && x.Active == true);

                    if (follow != null)
                    {
                        result.Followed = true;
                    }
                    else
                    {
                        result.Followed = false;
                    }

                    result.FollowCount = followService.GetActive(x => x.FollowerId == user.Id).Count();

                    result.FollowedCount= user.Follows.Where(x => x.UserId == user.Id).Count();

                    result.PostCount = user.Posts.Count();

                    response = new ResponseModel<AspNetUserOveralViewModel>(true, "Thông tin tài khoản!", null, result);
                }
                else
                {
                    response = ResponseModel<AspNetUserOveralViewModel>.CreateErrorResponse("Thất bại khi tải thông tin tài khoản!", systemError);
                }

            }
            catch (Exception)
            {
                response = ResponseModel<AspNetUserOveralViewModel>.CreateErrorResponse("Thất bại khi tải thông tin tài khoản!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult UpdateProfile(AspNetUserViewModel model)
        {
            var service = this.Service<IAspNetUserService>();

            AspNetUser user = null;

            ResponseModel<AspNetUserOveralViewModel> response = null;

            List<String> errorList = new List<string>();

            try
            {
                user = service.FindUser(model.Id);

                if (user != null)
                {
                    AspNetUser userInfo = Mapper.Map<AspNetUser>(model);
                    if (ValidateUserInfo(errorList, userInfo))
                    {
                        user = service.UpdateUser(userInfo);

                        AspNetUserOveralViewModel result = PrepareAspNetUserOveralViewModel(user);

                        response = new ResponseModel<AspNetUserOveralViewModel>(true, "Thông tin của bạn đã được cập nhật!", null, result);
                    }
                    else
                    {
                        response = new ResponseModel<AspNetUserOveralViewModel>(false, "Thông tin của bạn chưa được cập nhật!", errorList);
                    }

                }
                else
                {
                    response = ResponseModel<AspNetUserOveralViewModel>.CreateErrorResponse("Thông tin của bạn chưa được cập nhật!", systemError);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<AspNetUserOveralViewModel>.CreateErrorResponse("Thông tin của bạn chưa được cập nhật!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ChangeAvatar(String userId, HttpPostedFileBase image)
        {
            var service = this.Service<IAspNetUserService>();

            ResponseModel<String> response = null;

            FileUploader uploader = new FileUploader();

            try
            {
                String path = uploader.UploadImage(image, userImagePath);

                String result = service.ChangeAvatar(userId, path);

                response = new ResponseModel<String>(true, "Ảnh đại diện của bạn đã được cập nhật!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<String>.CreateErrorResponse("Ảnh đại diện của bạn chưa được cập nhật!", systemError);
            }

            return Json(response);

        }

        [HttpPost]
        public ActionResult ChangeCover(String userId, HttpPostedFileBase image)
        {
            var service = this.Service<IAspNetUserService>();

            ResponseModel<String> response = null;

            FileUploader uploader = new FileUploader();

            try
            {
                String path = uploader.UploadImage(image, userImagePath);

                String result = service.ChangeCover(userId, path);

                response = new ResponseModel<String>(true, "Ảnh bìa của bạn đã được cập nhật!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<String>.CreateErrorResponse("Ảnh bìa của bạn chưa được cập nhật!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> Login(String username, String password)
        {
            var loginResult = await SignInManager.PasswordSignInAsync(username, password, true, false);

            ResponseModel<UserLoginViewModel> response = null;

            switch (loginResult)
            {
                case SignInStatus.Success:
                    var service = this.Service<IAspNetUserService>();

                    AspNetUser user = service.FindUserByUserName(username);

                    bool MobileAuthorized = true;

                    foreach (var role in user.AspNetRoles)
                    {
                        if (role.Id.Equals(AdminRoleId))
                        {
                            MobileAuthorized = false;
                            response = ResponseModel<UserLoginViewModel>.CreateErrorResponse("Đăng nhập thất bại!", Utils.GetEnumDescription(UserRole.Admin) + " không thể đăng nhập trên thiết bị điện thoại");
                        }
                        else if (role.Id.Equals(ModeratorRoleId))
                        {
                            MobileAuthorized = false;
                            response = ResponseModel<UserLoginViewModel>.CreateErrorResponse("Đăng nhập thất bại!", Utils.GetEnumDescription(UserRole.Moderator) + " không thể đăng nhập trên thiết bị điện thoại");
                        }

                    }

                    if (MobileAuthorized)
                    {
                        UserLoginViewModel result = Mapper.Map<UserLoginViewModel>(user);

                        response = new ResponseModel<UserLoginViewModel>(true, "Đăng nhập thành công!", null, result);
                    }


                    break;
                case SignInStatus.LockedOut:
                    response = ResponseModel<UserLoginViewModel>.CreateErrorResponse("Đăng nhập thất bại!", "Locked out");

                    break;
                case SignInStatus.RequiresVerification:
                //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                    response = ResponseModel<UserLoginViewModel>.CreateErrorResponse("Đăng nhập thất bại!", "Tên tài khoản hoặc mật khẩu không chính xác");

                    break;
            }
            return Json(response);

        }

        [HttpPost]
        public async Task<ActionResult> Register(AspNetUserRegisterViewModel model)
        {
            List<String> errorList = new List<string>();

            ResponseModel<AspNetUserRegisterViewModel> response = null;

            try
            {
                bool isValidAccount = ValidateRegisterInfo(errorList, model);
                if (isValidAccount)
                {
                    var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                    var registerResult = await UserManager.CreateAsync(user, model.Password);
                    if (registerResult.Succeeded)
                    {

                        var userService = this.Service<IAspNetUserService>();
                        var userEntity = userService.FindUserByUserName(user.UserName);
                        userEntity.FullName = model.FullName;
                        userEntity.Active = true;
                        if (model.PlaceOwner != null)
                        {
                            userEntity.Status = (int)UserStatus.Pending;
                            UserManager.AddToRole(userEntity.Id, Utils.GetEnumDescription(UserRole.PlaceOwner));
                        }
                        else
                        {
                            userEntity.Status = (int)UserStatus.Active;
                            UserManager.AddToRole(userEntity.Id, Utils.GetEnumDescription(UserRole.Member));
                        }
                        userService.Update(userEntity);
                        var hobbyService = this.Service<IHobbyService>();
                        hobbyService.SaveHobbyForUser(userEntity.Id, model.Hobby);
                        response = new ResponseModel<AspNetUserRegisterViewModel>(true, "Bạn đã đăng ký thành công", null, model);
                    }
                    else
                    {
                        AddErrors(registerResult, errorList);
                        response = new ResponseModel<AspNetUserRegisterViewModel>(false, "Đăng ký không thành công", errorList, model);
                    }

                }
                else
                {
                    response = new ResponseModel<AspNetUserRegisterViewModel>(false, "Đăng ký không thành công", errorList, model);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<AspNetUserRegisterViewModel>.CreateErrorResponse("Đăng ký không thành công", systemError);
            }

            return Json(response);

        }

        [HttpPost]
        public ActionResult SaveToken(String userId, String token)
        {
            var tokenService = this.Service<IFirebaseTokenService>();

            var userService = this.Service<IAspNetUserService>();

            ResponseModel<bool> response = null;

            try {
                FirebaseToken firebaseToken = tokenService.FirstOrDefault(x => x.UserId.Equals(userId) && x.Token.Equals(token));

                AspNetUser user = userService.FirstOrDefaultActive(x=> x.Id.Equals(userId));

                if (firebaseToken == null)
                {
                    firebaseToken = new FirebaseToken();
                    firebaseToken.UserId = userId;
                    firebaseToken.Token = token;
                    tokenService.Create(firebaseToken);
                    tokenService.Save();

                }
                else
                {
                    if (user != null) {
                        firebaseToken.UserId = user.Id;
                        tokenService.Update(firebaseToken);
                        tokenService.Save();
                    }
                }

                response = new ResponseModel<bool>(true, "Token đã được tạo", null);

            } catch (Exception) {
                response = ResponseModel<bool>.CreateErrorResponse("Tạo token thất bại",systemError);
            }
            return Json(response);
        }

        private bool ValidateUserInfo(List<String> errorList, AspNetUser userInfo)
        {
            bool result = true;

            String emailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            String phoneNumberRegex = @"^([0-9]{10,12})$";

            if (!Regex.IsMatch(userInfo.Email, emailRegex))
            {
                errorList.Add("Email không hợp lệ!");
                result = false;
            }

            if (userInfo.Birthday > DateTime.Now)
            {
                errorList.Add("Ngày sinh không hợp lệ");
                result = false;
            }

            if (!Regex.IsMatch(userInfo.PhoneNumber, phoneNumberRegex))
            {
                errorList.Add("Số điện thoại không hợp lệ!");
                result = false;
            }

            return result;
        }

        private bool ValidateRegisterInfo(List<String> errorList, AspNetUserRegisterViewModel model)
        {
            var service = this.Service<IAspNetUserService>();

            AspNetUser user = service.FirstOrDefaultActive(x=> x.UserName == model.UserName);

            bool result = true;

            String emailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            String phoneNumberRegex = @"^([0-9]{10,12})$";

            if (user != null)
            {
                errorList.Add("Tên tài khoản đã tồn tại!");
                result = false;
            }

            if (!Regex.IsMatch(model.Email, emailRegex))
            {
                errorList.Add("Email không hợp lệ!");
                result = false;
            }

            if (!Regex.IsMatch(model.PhoneNumber, phoneNumberRegex))
            {
                errorList.Add("Số điện thoại không hợp lệ!");
                result = false;
            }

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                errorList.Add("Mật khẩu và mật khẩu nhập lại không đúng!");
                result = false;
            }

            if (model.Password.Length < 6)
            {
                errorList.Add("Mật khẩu phải có ít nhất 6 ký tự.");
                result = false;
            }

            if (model.UserName.Length < 6)
            {
                errorList.Add("Tên tài khoản phải có ít nhất 6 ký tự.");
                result = false;
            }

            return result;
        }
        private void AddErrors(IdentityResult result, List<String> errorList)
        {
            foreach (var error in result.Errors)
            {
                if (error.EndsWith("is already taken."))
                {
                    errorList.Add(error.Replace("is already taken.", "đã tồn tại.").Replace("Name", "Tên tài khoản"));
                }
            }
        }

        private List<AspNetUser> FilterMember(List<AspNetUser> userList)
        {
            for (int i = 0; i < userList.Count; i++)
            {
                var user = userList[i];
                List<AspNetRole> roles = user.AspNetRoles.ToList();

                foreach (var role in roles)
                {
                    if (role.Id.Equals(AdminRoleId) || role.Id.Equals(ModeratorRoleId))
                    {
                        userList.Remove(user);
                    }
                }
            }

            return userList;
        }

        private AspNetUserOveralViewModel PrepareAspNetUserOveralViewModel(AspNetUser user)
        {
            AspNetUserOveralViewModel result = Mapper.Map<AspNetUserOveralViewModel>(user);

            if (result.Hobbies != null)
            {
                var service = this.Service<ISportService>();
                foreach (var hobby in result.Hobbies)
                {

                    hobby.SportName = service.GetSportName(hobby.SportId);
                }
            }

            if (user.Gender != null)
            {
                result.Gender = Utils.GetEnumDescription((Gender)user.Gender);
            }

            if (user.Birthday != null)
            {
                result.BirthdayString = result.Birthday.ToString("dd/MM/yyyy");
            }

            return result;
        }
    }
}