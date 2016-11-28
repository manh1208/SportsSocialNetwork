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
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class InvitationController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult GetSentInvitation(string userId) {
            var invitationService = this.Service<IInvitationService>();

            ResponseModel<List<InvitationViewModel>> response = null;

            try {
                List<Invitation> inviList = invitationService.GetActive(x=> x.SenderId.Equals(userId)).ToList();

                List<InvitationViewModel> result = new List<InvitationViewModel>();

                foreach(var invi in inviList)
                {
                    result.Add(PrepareInvitationViewModel(invi));
                }

                response = new ResponseModel<List<InvitationViewModel>>(true, "Lời mời bạn đã gửi:", null, result);

            } catch (Exception) {
                response = ResponseModel<List<InvitationViewModel>>.CreateErrorResponse("Không thể tải lời mời", systemError);
            }
            return Json(response);
        }

        public ActionResult GetReceivedInvitation(string userId) {
            var invitationService = this.Service<IInvitationService>();

            var userInviService = this.Service<IUserInvitationService>();


            ResponseModel<List<InvitationViewModel>> response = null;

            try {
                List<UserInvitation> list = userInviService.GetActive(x => x.ReceiverId.Equals(userId)).ToList();

                List<Invitation> inviList = new List<Invitation>();

                foreach (var l in list)
                {
                    inviList.Add(l.Invitation);
                }

                List<InvitationViewModel> result = new List<InvitationViewModel>();

                foreach (var invi in inviList)
                {
                    result.Add(PrepareInvitationViewModelForReceiver(invi,userId));
                }

                response = new ResponseModel<List<InvitationViewModel>>(true, "Lời mời của bạn:", null, result);
            }
            catch (Exception) {
                response = ResponseModel<List<InvitationViewModel>>.CreateErrorResponse("Không thể tải lời mời", systemError);
            }
            return Json(response);
        }

        private InvitationViewModel PrepareInvitationViewModel(Invitation invitation) {
            InvitationViewModel result = Mapper.Map<InvitationViewModel>(invitation);

            foreach (var u in result.UserInvitations)
            {
                foreach (var u2 in invitation.UserInvitations)
                {
                    if (u.AspNetUser.Id.Equals(u2.AspNetUser.Id))
                    {
                        u.AspNetUser = PrepareAspNetUserOveralViewModel(u2.AspNetUser);
                    }
                }

            }

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.Sender = PrepareAspNetUserOveralViewModel(invitation.AspNetUser);

            return result;

        }

        [HttpPost]
        public ActionResult AcceptInvitation(int id)
        {
            var service = this.Service<IUserInvitationService>();

            ResponseModel<bool> response = null;
            try {
                UserInvitation invi = service.FirstOrDefaultActive(x => x.Id == id);

                invi.Accepted = true;

                service.Update(invi);

                service.Save();

                response = new ResponseModel<bool>(true, "Đã chấp nhận lời mời", null);
            } catch (Exception) {
                response = ResponseModel<bool>.CreateErrorResponse("Thao tác thất bại", systemError);
            }
            return Json(response);

        }

        [HttpPost]
        public ActionResult RefuseInvitation(int id)
        {
            var service = this.Service<IUserInvitationService>();

            ResponseModel<bool> response = null;
            try
            {
                UserInvitation invi = service.FirstOrDefaultActive(x => x.Id == id);

                invi.Accepted = false;

                service.Update(invi);

                service.Save();

                response = new ResponseModel<bool>(true, "Đã từ chối lời mời", null);
            }
            catch (Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Thao tác thất bại", systemError);
            }
            return Json(response);

        }

        private InvitationViewModel PrepareInvitationViewModelForReceiver(Invitation invitation, string currentUserId)
        {
            InvitationViewModel result = Mapper.Map<InvitationViewModel>(invitation);

            for (int i=0;i< result.UserInvitations.Count;i++)
            {
                var u = result.UserInvitations[i];

                if (!(u.AspNetUser.Id.Equals(currentUserId)))
                {
                    result.UserInvitations.Remove(u);
                }
            }

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.Sender = PrepareAspNetUserOveralViewModel(invitation.AspNetUser);

            return result;

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