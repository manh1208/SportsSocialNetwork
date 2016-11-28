using SkyWeb.DatVM.Data;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.Admin.Models;
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

namespace SportsSocialNetwork.Areas.Admin.Controllers
{


    [MyAuthorize(Roles = IdentityMultipleRoles.Admin)]
    public class PlaceController : BaseController
    {
        // GET: Admin/Place
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexList(JQueryDataTableParamModel request)
        {

            var service = this.Service<IPlaceService>();
            var totalRecord = 0;
            var count = 1;

            var results = service.GetPlaces(request, out totalRecord)
                .AsEnumerable()
                .Select(a => new IConvertible[] {
                        count++,
                        a.Name,
                        a.Address,
                        a.PhoneNumber,
                        a.AspNetUser.FullName,
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

        public ActionResult Detail(int id)
        {
            var model = new Models.PlaceDetailViewModel();
            var service = this.Service<IPlaceService>();
            var place = service.Get(id);
            if (place == null)
            {
                return this.IdNotFound();
            }else
            {
                model = Mapper.Map<Models.PlaceDetailViewModel>(place);
                model.CreateAddressString();
            }
            return this.PartialView(model);
        }
        public ActionResult Update(int id)
        {
            
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult RejectPlace(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IPlaceService>();
            var place = service.Get(id);
            if (place == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    place.Approve = false;
                    place.Status = (int)PlaceStatus.Unapproved;
                    service.Update(place);
                    string subject = "[SSN] - Không Chấp nhận Sân";
                    string body = "Hi <strong>" + place.AspNetUser.FullName + "</strong>" +
                        "<br/><br>/Sân " + place.Name + " của bạn không được chấp nhận vì một số thông tin không hợp lệ" +
                        "<br/> Tên sân : <strong>" + place.Name + "</strong>" +
                         "<br/> số điện thoại : <strong>" + place.PhoneNumber + "</strong>" +
                          "<br/> Địa chỉ : <strong>" + place.Address + " - " + place.Ward + " - " + place.District + " - " + place.City + "</strong>" +
                           "<br/> Tên sân : <strong>" + place.Name + "</strong>";
                    EmailSender.Send(Setting.CREDENTIAL_EMAIL, new string[] { place.AspNetUser.Email }, null, null, subject, body, true);
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
        public ActionResult ApprovePlace(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IPlaceService>();
            var place = service.Get(id);
            if (place == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    place.Approve = true;
                    place.Status = (int)PlaceStatus.Active;
                    service.Update(place);
                    string subject = "[SSN] - Chấp nhận Sân";
                    string body = "Hi <strong>" + place.AspNetUser.FullName + "</strong>" +
                        "<br/><br>/Sân " + place.Name + " của bạn đã được chấp nhận" +
                        " Vui lòng vào <a href=\"" + Url.Action("Login", "Account", new { area = "" }) + "\">link</a> để đăng nhập" +
                        "<br/> Tên sân : <strong>" + place.Name + "</strong>" +
                         "<br/> số điện thoại : <strong>" + place.PhoneNumber + "</strong>" +
                          "<br/> Địa chỉ : <strong>" + place.Address + " - " + place.Ward + " - " + place.District + " - " + place.City + "</strong>" +
                           "<br/> Tên sân : <strong>" + place.Name + "</strong>";
                    EmailSender.Send(Setting.CREDENTIAL_EMAIL, new string[] { place.AspNetUser.Email }, null, null, subject, body, true);
                    result.Succeed = true;
                } catch (Exception)
                {
                    result.Succeed = false;
                }
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IPlaceService>();
            var place = service.Get(id);
            if (place == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    service.Deactivate(place);
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