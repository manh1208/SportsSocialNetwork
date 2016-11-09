using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using SportsSocialNetwork.Models.Identity;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
    public class InvitationController : BaseController
    {
        // GET: Invitation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGroupChat()
        {
            var result = new AjaxOperationResult<List<InvitationViewModel>>();
            var userId = User.Identity.GetUserId();
            var inviService = this.Service<IInvitationService>();
            var invitations = inviService.GetActive(p => (p.SenderId == userId || p.UserInvitations.Where(f => f.ReceiverId == userId && f.Accepted != false).ToList().Count>0)).OrderByDescending(p => p.CreateDate).ToList();
            List<InvitationViewModel> rsList = new List<InvitationViewModel>();
            if (invitations != null)
            {
                foreach(var item in invitations)
                {
                    InvitationViewModel rs = Mapper.Map<InvitationViewModel>(item);
                    rs.Host = item.AspNetUser.FullName;
                    var tmp = item.UserInvitations.Where(p => p.Accepted != true).ToList().Count;
                    rs.numOfUser = item.UserInvitations.Count - tmp + 1;
                    rsList.Add(rs);
                }
                result.AdditionalData = rsList;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            
            return Json(result);
        }
        public ActionResult GetUserListInGroupChat(int invitationId)
        {
            var result = new AjaxOperationResult<List<AspNetUserSimpleModel>>();
            var userService = this.Service<IAspNetUserService>();
            var userInviService = this.Service<IUserInvitationService>();
            var inviService = this.Service<IInvitationService>();
            List<AspNetUserSimpleModel> resultList = new List<AspNetUserSimpleModel>();
            var uIn = userInviService.GetActive(p => p.InvitationId == invitationId).ToList();

            if (uIn != null)
            {
                foreach(var item in uIn)
                {
                    var user = userService.FirstOrDefaultActive(p => p.Id == item.ReceiverId);
                    AspNetUserSimpleModel model = Mapper.Map<AspNetUserSimpleModel>(user);
                    resultList.Add(model);
                }
            }
            var invi = inviService.FirstOrDefaultActive(p => p.Id == invitationId);
            var userId = invi.SenderId;
            var curUser = userService.FirstOrDefaultActive(p => p.Id == userId);
            AspNetUserSimpleModel userModel = Mapper.Map<AspNetUserSimpleModel>(curUser);
            resultList.Add(userModel);
            result.AdditionalData = resultList;
            result.Succeed = true;
            return Json(result);
        }
    }
}