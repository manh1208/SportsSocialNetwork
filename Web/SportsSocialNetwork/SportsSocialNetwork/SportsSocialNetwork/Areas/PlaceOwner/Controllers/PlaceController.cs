using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    public class PlaceController : Controller
    {
        // GET: PlaceOwner/Place
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreatePlace()
        {
            return View();
        }

        public ActionResult PlaceDetail(int? id)
        {
            var _placeService = this.Service<IPlaceService>();
            Place place = _placeService.FirstOrDefault(p => p.Id == id.Value);
            var _placeImageService = this.Service<IPlaceImageService>();
            List<PlaceImage> placeImages = _placeImageService.Get(p => p.PlaceId == id.Value).ToList();

            ViewBag.placeImages = placeImages;

            return View(place);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createPlace(Place place, IEnumerable<HttpPostedFileBase> uploadImages)
        {
            var _placeService = this.Service<IPlaceService>();
            _placeService.savePlace(place);
            if(uploadImages != null && uploadImages.ToList().Count > 0)
            {
                var _placeImageService = this.Service<IPlaceImageService>();
                _placeImageService.saveImage(place.Id, uploadImages);
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult updatePlace(Place place, IEnumerable<HttpPostedFileBase> uploadImages)
        {
            var _placeService = this.Service<IPlaceService>();
            _placeService.savePlace(place);
            if (uploadImages != null && uploadImages.ToList().Count > 0)
            {
                var _placeImageService = this.Service<IPlaceImageService>();
                _placeImageService.saveImage(place.Id, uploadImages);
            }

            return RedirectToAction("PlaceDetail", new RouteValueDictionary(
                new { controller = "Place", action = "PlaceDetail", id = place.Id }));
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

        public ActionResult GetData(jQueryDataTableParamModel param)
        {
            //var blogPostList = _blogPostService.GetBlogPostbyStoreId();
            var _placeService = this.Service<IPlaceService>();
            var placeList = _placeService.Get();
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
                p.City,
                p.Id
            }.ToArray());

            return Json(new
            {
                param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = filteredListItems.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

        }
    }
}