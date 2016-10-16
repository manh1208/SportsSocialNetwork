using SkyWeb.DatVM.Mvc;
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

namespace SportsSocialNetwork.Controllers
{
    public class EventController : BaseController
    {
        // GET: Event
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
            var viewModel = new SearchPlaceViewModel();
            return View(viewModel);
        }

        public ActionResult ViewDetail(int? id)
        {
            var _eventService = this.Service<IEventService>();
            var _placeService = this.Service<IPlaceService>();
            var entity = _eventService.FirstOrDefaultActive(p => p.Id == id);
            var events = new EventViewModel(entity);
            ViewBag.EventStatus = Utils.GetEnumDescription((EventStatus)events.Status);
            ViewBag.Place = _placeService.FirstOrDefaultActive(p => p.Id == events.PlaceId);
            return View(events);
        }
    }
}