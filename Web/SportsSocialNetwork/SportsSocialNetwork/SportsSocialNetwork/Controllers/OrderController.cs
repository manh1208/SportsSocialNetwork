using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SportsSocialNetwork.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Index(int id)
        {
            
            var model = new OrderViewModel();
            //model.CreateDate = DateTime.Today;
            model.FieldId = id;
            return this.PartialView(model);
        }

        [HttpPost]
        public ActionResult ConfirmOrder(OrderViewModel order)
        {
            var fieldId = order.FieldId;
            TimeSpan startTime = order.StartTime.TimeOfDay;
            TimeSpan endTime = order.EndTime.TimeOfDay;
            var _timeBlockService = this.Service<ITimeBlockService>();
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            var field = _fieldService.FirstOrDefaultActive(p => p.Id == fieldId);
            Place place = new Place();
            if (field != null)
            {
                place = _placeService.FirstOrDefaultActive(p => p.Id == field.PlaceId);
                ViewBag.field = field;
            }
            //var _userService = this.Service<IAspNetUserService>();
            order.Price = _timeBlockService.calPrice(fieldId, startTime, endTime);
            ViewBag.playDate = order.CreateDate;
            order.CreateDate = DateTime.Today;
            if (place != null)
            {
                ViewBag.place = place;
            }
            return View(order);
        }

        /*public ActionResult CreateOrder(OrderViewModel order)
        {
            User.Identity.GetUserId();
            
        }*/
    }
}