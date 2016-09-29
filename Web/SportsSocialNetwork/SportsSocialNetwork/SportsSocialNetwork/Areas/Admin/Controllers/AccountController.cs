using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.Admin.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using SportsSocialNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexList(JQueryDataTableParamModel request)
        {
            var service = this.Service<IAspNetUserService>();
            var totalRecord = 0;
            var count = 1;
            var results = service.GetUsers(request, out totalRecord)
                .AsEnumerable()
                .Select(a => new IConvertible[] {
                        count++,
                        a.FullName,
                        a.PhoneNumber,
                        a.Email,
                        a.UserName,
                        a.AspNetRoles.FirstOrDefault().Name,
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
        public ActionResult GetDetail(string id)
        {
            var result = new AjaxOperationResult<AccountDetailViewModel>();
            var service = this.Service<IAspNetUserService>();
            var entity = service.FindUser(id);
            if (entity == null)
            {
                return this.IdNotFound();
            }
            else
            {
                result.Succeed = true;
                var model = Mapper.Map<AccountDetailViewModel>(entity);
                Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
                Province province = vietnam.VietNamese.Where(p => p.Name.Equals(model.City)).FirstOrDefault();

                if (province != null)
                {
                    model.City = province.Type + " " + province.Name;
                    District district = province.Districts.Where(d => d.Name.Equals(model.District)).FirstOrDefault();
                    if (district != null)
                    {
                        model.District = district.Type + " " + district.Name;
                        Ward ward = district.Wards.Where(d => d.Name.Equals(model.Ward)).FirstOrDefault();
                        if (ward != null)
                        {
                            model.Ward = ward.Type + " " + ward.Name;
                        }
                    }

                }

                model.CreateAddressString();
                model.CreateBirthdayString();
                model.CreateRole();
                model.GenderString = Utils.GetEnumDescription((Gender)model.Gender);
                result.AdditionalData = model;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult PrepareUpdate(string id)
        {
            var result = new AjaxOperationResult<AccountUpdateViewModel>();
            var user_service = this.Service<IAspNetUserService>();
            var role_service = this.Service<IAspNetRoleService>();
            var entity = user_service.FindUser(id);
            if (entity == null)
            {
                return this.IdNotFound();
            }
            else
            {
                var detail = Mapper.Map<AccountDetailViewModel>(entity);
                detail.CreateAddressString();
                detail.CreateBirthdayString();
                detail.CreateRole();
                Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
                Province province = vietnam.VietNamese.Where(p => p.Name.Equals(detail.City)).FirstOrDefault();
                District district = province.Districts.Where(d => d.Name.Equals(detail.District)).FirstOrDefault();

                var provinces = vietnam.VietNamese.Select(p =>
                                new SelectListItem
                                {
                                    Text = p.Type + " " + p.Name,
                                    Value = p.Name
                                })
                                .OrderBy(p => p.Value);

                var districts = vietnam.VietNamese.Where(p => p.Name.Equals(detail.City))
                                               .FirstOrDefault()
                                               .Districts
                                               .Select(d =>
                                               new SelectListItem
                                               {
                                                   Text = d.Type + " " + d.Name,
                                                   Value = d.Name
                                               })
                                               .OrderBy(d => d.Value);
                var wards = vietnam.VietNamese[0].Districts.ToList()[0]
                                              .Wards
                                              .Select(w =>
                                              new SelectListItem
                                              {
                                                  Text = w.Type + " " + w.Name,
                                                  Value = w.Name
                                              })
                                              .OrderBy(w => w.Value);


                if (province != null)
                {
                    districts = province.Districts.Select(d =>
                                               new SelectListItem
                                               {
                                                   Text = d.Type + " " + d.Name,
                                                   Value = d.Name
                                               })
                                               .OrderBy(d => d.Value);
                    if (district != null)
                    {
                        wards = district.Wards.Select(w =>
                                              new SelectListItem
                                              {
                                                  Text = w.Type + " " + w.Name,
                                                  Value = w.Name
                                              })
                                              .OrderBy(w => w.Value);
                    }
                }

                var roles = role_service.Get()
                                        .Select(r =>
                                        new SelectListItem
                                        {
                                            Text = r.Name,
                                            Value = r.Id
                                        });

                var model = new AccountUpdateViewModel
                {
                    Detail = detail,
                    Provinces = provinces,
                    Districts = districts,
                    Wards = wards,
                    Roles = roles
                };
                result.AdditionalData = model;
                result.Succeed = true;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetDistrict(string provinceName)
        {
            var result = new AjaxOperationResult<IEnumerable<SelectListItem>>();
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            var districts = vietnam.VietNamese.Where(p => p.Name.Equals(provinceName))
                                                .FirstOrDefault()
                                                .Districts
                                                .Select(d =>
                                                new SelectListItem
                                                {
                                                    Text = d.Type + " " + d.Name,
                                                    Value = d.Name
                                                })
                                                .OrderBy(d => d.Value);
            if (districts.Count() > 0)
            {
                result.Succeed = true;
                result.AdditionalData = districts;
            }
            else
            {
                result.Succeed = false;

            }

            return Json(result);
        }

        public ActionResult GetWard(string provinceName, string districtName)
        {
            var result = new AjaxOperationResult<IEnumerable<SelectListItem>>();
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            var wards = vietnam.VietNamese.Where(p => p.Name.Equals(provinceName))
                                               .FirstOrDefault()
                                               .Districts
                                               .Where(d => d.Name.Equals(districtName))
                                               .FirstOrDefault()
                                               .Wards
                                               .Select(w =>
                                               new SelectListItem
                                               {
                                                   Text = w.Type + " " + w.Name,
                                                   Value = w.Name
                                               })
                                               .OrderBy(w => w.Value);
            if (wards.Count() > 0)
            {
                result.Succeed = true;
                result.AdditionalData = wards;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        public ActionResult UpdateAccount(AspNetUserViewModel model)
        {
            var result = new AjaxOperationResult();
            try
            {
                var service = this.Service<IAspNetUserService>();

                AspNetUser user = service.Get(model.Id);
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.City = model.City;
                user.District = model.District;
                user.Ward = model.Ward;
                user.Birthday = model.Birthday;
                user.Gender = model.Gender;
                service.Update(user);
                result.Succeed = true;
            }
            catch (Exception)
            {
                result.Succeed = false;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteAccount(string id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IAspNetUserService>();
            var user = service.FindUser(id);
            if (user == null)
            {
                return this.IdNotFound();
            }
            else
            {
                service.DeactivateUser(user);
            }

            return Json(result);
        }
    }
}