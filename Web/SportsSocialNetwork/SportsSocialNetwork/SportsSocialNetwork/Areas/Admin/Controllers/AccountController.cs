using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.Admin.Models;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Identity;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using SportsSocialNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SportsSocialNetwork.Areas.Admin.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.Admin)]
    public class AccountController : BaseController
    {

        private ApplicationUserManager _userManager;


        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
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

        public ActionResult Detail(string id)
        {
            var service = this.Service<IAspNetUserService>();
            var entity = service.FindUser(id);
            AccountDetailViewModel model;
            if (entity == null)
            {
                return this.IdNotFound();
            }
            else
            {
              
                model = Mapper.Map<AccountDetailViewModel>(entity);
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
                if (model.Gender.HasValue) { 
                model.GenderString = Utils.GetEnumDescription((Gender)model.Gender);
                } 
            }
            return this.PartialView(model) ;
        }

        public ActionResult Update(string id)
        {
            var user_service = this.Service<IAspNetUserService>();
            var role_service = this.Service<IAspNetRoleService>();
            var entity = user_service.FindUser(id);
            UpdateAccountViewModel detail;
            if (entity == null)
            {
                return this.IdNotFound();
            }
            else
            {
                detail = Mapper.Map<UpdateAccountViewModel>(entity);
                //detail.CreateAddressString();
                //detail.CreateBirthdayString();
                detail.CreateRole();
                Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
                Province province = vietnam.VietNamese.Where(p => p.Name.Equals(detail.City)).FirstOrDefault();


                var provinces = vietnam.VietNamese.Select(p =>
                                new SelectListItem
                                {
                                    Text = p.Type + " " + p.Name,
                                    Value = p.Name
                                })
                                .OrderBy(p => p.Value);

                IOrderedEnumerable<SelectListItem> districts = new List<SelectListItem>().OrderBy(d=>d.Value);
                IOrderedEnumerable<SelectListItem> wards =  new List<SelectListItem>().OrderBy(d => d.Value);
                if (province != null)
                {
                    districts = province.Districts.Select(d =>
                                               new SelectListItem
                                               {
                                                   Text = d.Type + " " + d.Name,
                                                   Value = d.Name
                                               })
                                               .OrderBy(d => d.Value);
                    District district = province.Districts.Where(d => d.Name.Equals(detail.District)).FirstOrDefault();
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

                var roles = role_service.Get().ToArray()
                                        .Select(r =>
                                        new SelectListItem
                                        {
                                            Text = r.Name,
                                            Value = r.Id
                                        }).OrderBy(r=>r.Value);
                provinces.ToList().Add(new SelectListItem { Text = "", Value = " " });
                districts.ToList().Add(new SelectListItem { Text = "", Value = " " });
                wards.ToList().Add(new SelectListItem { Text = "", Value = " " });
                roles.ToList().Add(new SelectListItem { Text = "", Value = " " });
                ViewBag.Provinces = provinces;
                ViewBag.Districts = districts;
                ViewBag.Wards = wards;
                ViewBag.Roles = roles;    
            }                     
                                  
            return this.PartialView(detail);
        }

        [HttpPost]
        public ActionResult GetDistrict(string provinceName)
        {
            var result = new AjaxOperationResult<IEnumerable<SelectListItem>>();
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            Province province = vietnam.VietNamese.Where(p => p.Name.Equals(provinceName)).FirstOrDefault();
            IOrderedEnumerable<SelectListItem> districts = null;
            if (province != null)
            {
                districts = province.Districts.Select(d =>
                                                    new SelectListItem
                                                    {
                                                        Text = d.Type + " " + d.Name,
                                                        Value = d.Name
                                                    })
                                                    .OrderBy(d => d.Value);
            }
            
                result.Succeed = true;
                result.AdditionalData = districts;
           

            return Json(result);
        }

        public ActionResult GetWard(string provinceName, string districtName)
        {
            var result = new AjaxOperationResult<IEnumerable<SelectListItem>>();
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            Province province = vietnam.VietNamese.Where(p => p.Name.Equals(provinceName)).FirstOrDefault();
            IOrderedEnumerable<SelectListItem> wards = null;
            if (province != null) {
                District district = province.Districts
                                            .Where(d => d.Name.Equals(districtName))
                                            .FirstOrDefault();
                if (district != null)
                {
                    wards =district.Wards.Select(w =>
                                              new SelectListItem
                                              {
                                                  Text = w.Type + " " + w.Name,
                                                  Value = w.Name
                                              })
                                              .OrderBy(w => w.Value);
                }
            }
            
          
                result.Succeed = true;
                result.AdditionalData = wards;
            return Json(result);
        }

        [HttpPost]
        public ActionResult Update(UpdateAccountViewModel model)
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
                
                var roleService = this.Service<IAspNetRoleService>();
                AspNetRole newRole = roleService.Get(model.RoleId);
                var roles = UserManager.GetRoles(user.Id).ToArray();
                
                //var oldRoles = Roles.GetRolesForUser(user.UserName);
                UserManager.RemoveFromRoles(user.Id, roles);
                UserManager.AddToRole(user.Id, newRole.Name);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.Succeed = false;
            }
            return Json(result);
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

        [HttpPost]
        public ActionResult BanUser(string id)
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

                    user.Status = (int)UserStatus.Banned;
                    service.Update(user);
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
        public ActionResult UnBanUser(string id)
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