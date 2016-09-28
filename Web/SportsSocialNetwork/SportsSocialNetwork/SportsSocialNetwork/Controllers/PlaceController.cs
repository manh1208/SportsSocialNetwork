using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using SportsSocialNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class PlaceController : BaseController
    {
        // GET: Place
        public ActionResult Index()
        {
            var _sportService = this.Service<ISportService>();
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            var sportList = _sportService.getAllSport();
            IEnumerable<SelectListItem> selectList = sportList.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToArray();
            ViewBag.SportList = selectList;

            var province = vietnam.VietNamese.ToList();
            IEnumerable<SelectListItem> provinceList = province.Select(m => new SelectListItem
            {
                Text = m.Type + " " + m.Name,
                Value = m.Name
            }).OrderBy(s => s.Value).ToArray();
            ViewBag.ProvinceList = provinceList;
            var viewModel =new  SearchPlaceViewModel();
            return View(viewModel);
        }

        public ActionResult GetDistrict(string provinceName)
        {
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            var province = vietnam.VietNamese.Where(p => p.Name.Equals(provinceName)).ToList();
            var district = province.First().Districts.ToList();
            IEnumerable<SelectListItem> districtList = district.Select(m => new SelectListItem
            {
                Text = m.Type +" "+ m.Name,
                Value = m.Name
            }).OrderBy(s => s.Value).ToArray();

            return Json(districtList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetData(JQueryDataTableParamModel param, string sport, string province, string district)
        {
            var _placeService = this.Service<IPlaceService>();
            var placeList = _placeService.getAllPlace();
            var sportId = -1;
            if (sport != null && sport != "")
            {
                sportId = Int32.Parse(sport);
            } 
            if(sportId != -1 || (province !=null && province != ""))
            {
                placeList = _placeService.getPlace(sportId, province, district);
            }
            
            IEnumerable<Place> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = placeList.Where(
                    d => (d.Name != null && d.Name.ToLower().Contains(param.sSearch.ToLower()))
                );
            }
            else
            {
                filteredListItems = placeList;
            }
            // Sort.
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc

            switch (sortColumnIndex)
            {
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(c => c.Name)
                        : filteredListItems.OrderByDescending(c => c.Name);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = displayedList.Select(c => new IConvertible[]{
                c.Id,
                c.PlaceImages.First().Image,
                c.Name,
                c.Description,
                c.Address,
                //c.Fields.First().FieldType.,
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