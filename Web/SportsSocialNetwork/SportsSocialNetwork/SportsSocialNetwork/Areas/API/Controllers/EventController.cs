using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class EventController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult GetAllEventOfPlaceOwner(String ownerId)
        {
            var service = this.Service<IEventService>();

            ResponseModel<List<EventOveralViewModel>> response = null;

            try
            {
                List<Event> eventList = service.GetAllPlaceOwnerEvent(ownerId);

                List<EventOveralViewModel> result = new List<EventOveralViewModel>();

                foreach (var e in eventList)
                {
                    result.Add(PrepareEventOveralViewModel(e));
                }

                response = new ResponseModel<List<EventOveralViewModel>>(true, "Danh sách cách sự kiện của bạn:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<EventOveralViewModel>>.CreateErrorResponse("Tải danh sách thất bại", systemError);

            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult GetAllEvent(string currentUserId, int skip, int take)
        {
            var service = this.Service<IEventService>();

            ResponseModel<List<EventOveralViewModel>> response = null;

            try
            {
                List<Event> eventList = service.GetAllEvent(skip,take);

                List<EventOveralViewModel> result = new List<EventOveralViewModel>();

                foreach (var e in eventList)
                {
                    result.Add(PrepareEventForMember(e,currentUserId));

                    List<Participation> list = e.Participations.ToList();

                    foreach (var l in list)
                    {
                        if (l.UserId.Equals(currentUserId)) {

                        }
                    }
                }

                response = new ResponseModel<List<EventOveralViewModel>>(true, "Danh sách cách sự kiện của bạn:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<EventOveralViewModel>>.CreateErrorResponse("Tải danh sách thất bại", systemError);

            }
            return Json(response);
        }

        private EventOveralViewModel PrepareEventOveralViewModel(Event e)
        {

            EventOveralViewModel result = Mapper.Map<EventOveralViewModel>(e);
            result.StartDateString = result.StartDate.ToString("dd/MM/yyyy");

            result.EndDateString = result.EndDate.ToString("dd/MM/yyyy");

            result.PlaceName = e.Place.Name;

            return result;
        }

        private EventOveralViewModel PrepareEventForMember(Event e, string currentUserId)
        {

            EventOveralViewModel result = Mapper.Map<EventOveralViewModel>(e);
            result.StartDateString = result.StartDate.ToString("dd/MM/yyyy");

            result.EndDateString = result.EndDate.ToString("dd/MM/yyyy");

            result.PlaceName = e.Place.Name;

            result.Joined = false;

            List<Participation> list = e.Participations.ToList();

            foreach (var l in list)
            {
                if (l.UserId.Equals(currentUserId))
                {
                    result.Joined = true;
                }
                
            }

            return result;
        }
    }
}