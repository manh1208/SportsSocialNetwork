using SkyWeb.DatVM.Mvc;
using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Areas.PlaceOwner.Models.ViewModels;
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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.PlaceOwner)]
    public class PlaceController : BaseController
    {
        // GET: PlaceOwner/Place
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreatePlace()
        {
            IOrderedEnumerable<SelectListItem> districts = new List<SelectListItem>().OrderBy(d => d.Value);
            IOrderedEnumerable<SelectListItem> wards = new List<SelectListItem>().OrderBy(d => d.Value);

            districts.ToList().Add(new SelectListItem { Text = "", Value = " " });
            wards.ToList().Add(new SelectListItem { Text = "", Value = " " });

            ViewBag.provinces = this.GetProvince();
            ViewBag.districts = districts;
            ViewBag.wards = wards;
            return View();
        }

        public ActionResult ModalDetail(int id)
        {
            var _placeService = this.Service<IPlaceService>();
            var _placeImageService = this.Service<IPlaceImageService>();

            Place place = _placeService.FirstOrDefaultActive(p => p.Id == id);
            List<PlaceImage> placeImages = _placeImageService.Get(i => i.PlaceId == id).ToList();

            Models.ViewModels.PlaceDetailViewModel model = Mapper.Map<Models.ViewModels.PlaceDetailViewModel>(place);
            model.placeImages = placeImages;
            model.generateAddress();

            return this.PartialView(model);
        }

        public ActionResult PlaceDetail(int? id)
        {
            var _placeService = this.Service<IPlaceService>();
            Place place = _placeService.FirstOrDefault(p => p.Id == id.Value);
            var _placeImageService = this.Service<IPlaceImageService>();
            List<PlaceImage> placeImages = _placeImageService.Get(p => p.PlaceId == id.Value).ToList();

            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            Province province = vietnam.VietNamese.Where(p => p.Name.Equals(place.City)).FirstOrDefault();


            var provinces = vietnam.VietNamese.Select(p =>
                            new SelectListItem
                            {
                                Text = p.Type + " " + p.Name,
                                Value = p.Name
                            })
                            .OrderBy(p => p.Value);

            IOrderedEnumerable<SelectListItem> districts = new List<SelectListItem>().OrderBy(d => d.Value);
            IOrderedEnumerable<SelectListItem> wards = new List<SelectListItem>().OrderBy(d => d.Value);
            if (province != null)
            {
                districts = province.Districts.Select(d =>
                                           new SelectListItem
                                           {
                                               Text = d.Type + " " + d.Name,
                                               Value = d.Name
                                           })
                                           .OrderBy(d => d.Value);
                District district = province.Districts.Where(d => d.Name.Equals(place.District)).FirstOrDefault();
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

            provinces.ToList().Add(new SelectListItem { Text = "", Value = " " });
            districts.ToList().Add(new SelectListItem { Text = "", Value = " " });
            wards.ToList().Add(new SelectListItem { Text = "", Value = " " });

            List<SelectListItem> statuss = new List<SelectListItem>();
            statuss.Add(new SelectListItem { Text = Utils.GetEnumDescription(PlaceStatus.Active), Value = Convert.ToString((int)PlaceStatus.Active) });
            statuss.Add(new SelectListItem { Text = Utils.GetEnumDescription(PlaceStatus.Repairing), Value = Convert.ToString((int)PlaceStatus.Repairing) });

            ViewBag.placeImages = placeImages;
            ViewBag.provinces = provinces;
            ViewBag.districts = districts;
            ViewBag.wards = wards;
            ViewBag.statusList = statuss;

            return View(place);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createPlace(Place place, IEnumerable<HttpPostedFileBase> uploadImages)
        {
            var _placeService = this.Service<IPlaceService>();
            _placeService.savePlace(place);
            if(uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
            {
                var _placeImageService = this.Service<IPlaceImageService>();
                _placeImageService.saveImage(place.Id, uploadImages);

                _placeImageService = this.Service<IPlaceImageService>();
                List<PlaceImage> listImage = _placeImageService.Get(i => i.PlaceId == place.Id).ToList();

                place.Avatar = listImage[0].Image;
            }
            _placeService.savePlace(place);
            
            return RedirectToAction("Index");
        }

        public ActionResult updatePlace(Place place, IEnumerable<HttpPostedFileBase> uploadImages)
        {
            var _placeService = this.Service<IPlaceService>();
            _placeService.savePlace(place);
            if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
            {
                var _placeImageService = this.Service<IPlaceImageService>();
                _placeImageService.saveImage(place.Id, uploadImages);
            }

            return RedirectToAction("PlaceDetail", new RouteValueDictionary(
                new { controller = "Place", action = "PlaceDetail", id = place.Id }));
        }

        public string updateAvatar(int id, int placeID)
        {
            var _placeImageService = this.Service<IPlaceImageService>();
            var _placeService = this.Service<IPlaceService>();

            PlaceImage pi = _placeImageService.FirstOrDefault(i => i.Id == id);
            if(pi != null)
            {
                Place place = _placeService.FirstOrDefaultActive(p => p.Id == placeID);
                if(place != null)
                {
                    place.Avatar = pi.Image;
                    _placeService.savePlace(place);
                    return "success";
                }
                return "false";
            }
            return "false";
        }

        public string deletePlace(int id)
        {
            var _placeService = this.Service<IPlaceService>();
            Place place = _placeService.FirstOrDefaultActive(p => p.Id == id);
            if (place != null)
            {
                _placeService.Deactivate(place);
                return "success";
            }
            return "false";
        }

        public string deleteImage(int id)
        {
            var _placeImageService = this.Service<IPlaceImageService>();
            PlaceImage placeImage = _placeImageService.FirstOrDefaultActive(p => p.Id == id);
            if (placeImage != null)
            {
                _placeImageService.Delete(placeImage);
                return "success";
            }
            return "false";
        }

        public ActionResult GetData(JQueryDataTableParamModel param)
        {
            string userID = Request["userID"];
            //var blogPostList = _blogPostService.GetBlogPostbyStoreId();
            var _placeService = this.Service<IPlaceService>();
            var placeList = _placeService.GetActive(p => p.UserId.Equals(userID));
            //IEnumerable<BlogPost> filteredListItems;
            IEnumerable<Place> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = placeList.Where(p => p.Name != null && p.Name.ToLower().Contains(param.sSearch.ToLower()));

                //filteredListItems = blogPostList.Where(
                //    d => (d.Title != null && d.Title.ToLower().Contains(param.sSearch.ToLower()))
                //    || (d.BlogCategory.Title != null && d.BlogCategory.Title.ToLower().Contains(param.sSearch.ToLower()))
                //    || (d.MetaDescription != null && d.MetaDescription.ToLower().Contains(param.sSearch.ToLower()))
                //);
            }
            else
            {
                //filteredListItems = blogPostList;
                filteredListItems = placeList;
            }
            // Sort.
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc

            switch (sortColumnIndex)
            {
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(p => p.Name)
                        : filteredListItems.OrderByDescending(p => p.Name);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = displayedList.Select(p => new IConvertible[]{
                //c.Id,
                //c.Image,
                //c.Title,
                //c.BlogCategoryId,
                //c.BlogCategory.Title,
                //c.Author,
                //c.Active,
                //c.Id

                p.Id,
                p.Name,
                p.Status,
                p.Address,
                p.Ward,
                p.District,
                p.City
            }.ToArray());

            return Json(new
            {
                param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = filteredListItems.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

        }

        public IOrderedEnumerable<SelectListItem> GetProvince()
        {
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            //Province province = vietnam.VietNamese.Where(p => p.Name.Equals(detail.City)).FirstOrDefault();

            IOrderedEnumerable<SelectListItem> provinces = vietnam.VietNamese.Select(p =>
                                new SelectListItem
                                {
                                    Text = p.Type + " " + p.Name,
                                    Value = p.Name
                                })
                                .OrderBy(p => p.Value);
            return provinces;
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
            if (province != null)
            {
                District district = province.Districts
                                            .Where(d => d.Name.Equals(districtName))
                                            .FirstOrDefault();
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


            result.Succeed = true;
            result.AdditionalData = wards;
            return Json(result);
        }
    }
}