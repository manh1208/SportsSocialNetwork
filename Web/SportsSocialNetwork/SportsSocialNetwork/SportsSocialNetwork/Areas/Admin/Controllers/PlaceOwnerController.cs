using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Identity;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SportsSocialNetwork.Areas.Admin.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.Admin)]
    public class PlaceOwnerController : BaseController
    {
        private ApplicationUserManager _userManager;


        public PlaceOwnerController()
        {
        }

        public PlaceOwnerController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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


        // GET: Admin/PlaceOwner
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexList(JQueryDataTableParamModel request)
        {
            var service = this.Service<IAspNetUserService>();
            var totalRecord = 0;
            var count = 1;
            
            var results = service.GetPlaceOwner(request, out totalRecord)
                .AsEnumerable()
                .Select(a => new IConvertible[] {
                        count++,
                        a.FullName,
                        a.PhoneNumber,
                        a.Email,
                        a.UserName,
                        a.Status,
                        a.Id

                }).ToArray();

            var model = new
            {
                draw = request.sEcho,
                data = results,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord
            };
            return Json(model);
        }

        [HttpPost]
        public ActionResult RejectPlaceOwner(string id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IAspNetUserService>();
            var user = service.Get(id);
            if (user == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                   
                    user.Status = (int)UserStatus.Unapproved;
                    service.Update(user);
                    //var roleService = this.Service<IAspNetRoleService>();
                    //AspNetRole newRole = roleService.Get(UserRole.Member.ToString("d"));
                    //var roles = UserManager.GetRoles(user.Id).ToArray();
                    ////var oldRoles = Roles.GetRolesForUser(user.UserName);
                    //UserManager.RemoveFromRoles(user.Id, roles);
                    //UserManager.AddToRole(user.Id, newRole.Name);
                    string subject = "[SSN] - Từ chối tài khoản";
                    string body = "Hi <strong>" + user.FullName + "</strong>" +
                        ",<br/><br/>Tài khoản của bạn đã bị từ chối vì một số thông tin không hợp lệ." +
                        "<br/> <strong>Tên tài khoản : " + user.UserName + "</strong>";
                    EmailSender.Send(Setting.CREDENTIAL_EMAIL, new string[] { user.Email }, null, null, subject, body, true);
                    result.Succeed = true;
                   

                }
                catch (Exception)
                {
                    result.Succeed = false;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult ApprovePlaceOwner(string id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IAspNetUserService>();
            var user = service.Get(id);
            if (user == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {

                    user.Status = (int)UserStatus.Active;
                    service.Update(user);
                    string subject = "[SSN] - Chấp nhận tài khoản";
                    string body = "Hi <strong>" + user.FullName + "</strong>"+
                        "<br/><br>/Tài khoản của bạn đã được chấp nhận" +
                        " Vui lòng vào <a href=\"" + Url.Action("Login", "Account", new { areas = "" }) + "\">link</a> để đăng nhập" +
                        "<br/> <strong>Tên tài khoản : " + user.UserName + "</strong>" +
                        "<br/> Password : Your password" + "</strong>";
                    EmailSender.Send(Setting.CREDENTIAL_EMAIL, new string[] { user.Email }, null, null, subject, body, true);
                    result.Succeed = true;
                   
                }
                catch (Exception)
                {
                    result.Succeed = false;
                }
            }

            return Json(result);
        }

    }
}