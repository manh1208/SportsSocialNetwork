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
                List<Event> eventList = service.GetAllPlaceOwnerEvent(ownerId).ToList();

                List<EventOveralViewModel> result = Mapper.Map<List<EventOveralViewModel>>(eventList);

                response = new ResponseModel<List<EventOveralViewModel>>(true, "Danh sách cách sự kiện của bạn:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<EventOveralViewModel>>.CreateErrorResponse("Tải danh sách thất bại", systemError);

            }
            return Json(response);
        }
    }
}