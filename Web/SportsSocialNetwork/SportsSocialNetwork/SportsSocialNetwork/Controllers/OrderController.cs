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
using SportsSocialNetwork.Areas.PlaceOwner.Models.ViewModels;

namespace SportsSocialNetwork.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public string CancelOrder(int id)
        {
            var _orderSeervice = this.Service<IOrderService>();
            Order order = _orderSeervice.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.Status = 0;
                _orderSeervice.Update(order);
                _orderSeervice.Save();
                return "success";
            }
            return "false";
        }

        public ActionResult OrderDetail(int id)
        {
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            var _orderService = this.Service<IOrderService>();
            Order order = _orderService.FirstOrDefault(o => o.Id == id);
            var fieldId = order.FieldId;
            var field = _fieldService.FirstOrDefaultActive(p => p.Id == fieldId);
            Place place = new Place();
            if (field != null)
            {
                place = _placeService.FirstOrDefaultActive(p => p.Id == field.PlaceId);
                ViewBag.field = field;
            }
            if (place != null)
            {
                ViewBag.place = place;
            }
            OrderViewModel model = new OrderViewModel(order);
            return this.PartialView(model);
        }

        public ActionResult GetData(JQueryDataTableParamModel param)
        {
            //var blogPostList = _blogPostService.GetBlogPostbyStoreId();
            var _orderService = this.Service<IOrderService>();
            var userId = User.Identity.GetUserId();
            var orderList = _orderService.GetActive(p => p.UserId == userId);
            //IEnumerable<BlogPost> filteredListItems;
            IEnumerable<Order> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = orderList.Where(o => o.Field.Name != null && o.Field.Name.ToLower().Contains(param.sSearch.ToLower()));

                //filteredListItems = blogPostList.Where(
                //    d => (d.Title != null && d.Title.ToLower().Contains(param.sSearch.ToLower()))
                //    || (d.BlogCategory.Title != null && d.BlogCategory.Title.ToLower().Contains(param.sSearch.ToLower()))
                //    || (d.MetaDescription != null && d.MetaDescription.ToLower().Contains(param.sSearch.ToLower()))
                //);
            }
            else
            {
                //filteredListItems = blogPostList;
                filteredListItems = orderList;
            }
            // Sort.
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc

            switch (sortColumnIndex)
            {
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.Field.Name)
                        : filteredListItems.OrderByDescending(o => o.Field.Name);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = displayedList.Select(o => new IConvertible[]{
                o.Id,
                o.Field.Name,
                o.CreateDate.ToString("dd/MM/yyyy HH:mm"),
                o.StartTime.ToString("HH:mm"),
                o.StartTime.ToString("dd/MM/yyyy"),
                o.EndTime.ToString("HH:mm"),
                o.PaidType,
                o.Status
            }.ToArray());

            return Json(new
            {
                param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = filteredListItems.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _Index(int id)
        {
            
            var model = new OrderViewModel();
            var userId = User.Identity.GetUserId();
            var _userService = this.Service<IAspNetUserService>();
            var img = _userService.FirstOrDefaultActive(p => p.Id == userId).AvatarImage;
            model.FieldId = id;
            if (String.IsNullOrEmpty(img))
            {
                img = "http://i57.servimg.com/u/f57/16/18/15/10/1104.png";
            }
            ViewBag.avatar = img;
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
            order.Price = _timeBlockService.calPrice(fieldId, startTime, endTime);
            ViewBag.playDate = order.CreateDate;
            order.CreateDate = DateTime.Today;
            if (place != null)
            {
                ViewBag.place = place;
            }
            var userId = User.Identity.GetUserId();
            var _userService = this.Service<IAspNetUserService>();
            var img = _userService.FirstOrDefaultActive(p => p.Id == userId).AvatarImage;
            if (String.IsNullOrEmpty(img))
            {
                img = "http://i57.servimg.com/u/f57/16/18/15/10/1104.png";
            }
            ViewBag.avatar = img;
            Session["order"] = order;
            return View(order);
        }


        public JsonResult CheckTimeValidInTimeBlock(int fieldId, string startTime, string endTime)
        {
            var result = new AjaxOperationResult();
            TimeSpan StartTime = TimeSpan.Parse(startTime);
            TimeSpan EndTime = TimeSpan.Parse(endTime);
            var _timeBlockService = this.Service<ITimeBlockService>();
            
            try
            {
                bool rs = _timeBlockService.checkTimeValid(fieldId, StartTime, EndTime);
                if (rs)
                {
                    result.Succeed = true;
                }
                else
                {
                    result.Succeed = false;
                }
               
            }
            catch (Exception)
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        public JsonResult CheckOrderTimeExist(int fieldId, string startTime, string endTime)
        {
            var result = new AjaxOperationResult();
            TimeSpan StartTime = TimeSpan.Parse(startTime);
            TimeSpan EndTime = TimeSpan.Parse(endTime);
            var _orderService = this.Service<IOrderService>();

            try
            {
                bool rs = _orderService.checkOrderTimeValid(fieldId, StartTime, EndTime);
                if (rs)
                {
                    result.Succeed = true;
                }
                else
                {
                    result.Succeed = false;
                }

            }
            catch (Exception)
            {
                result.Succeed = false;
            }
            return Json(result);
        }
        public JsonResult CreateOrder(int fieldId, string startTime, string endTime, string note, 
            double price, string paidType, string useDate)
        {
            TimeSpan StartTime = TimeSpan.Parse(startTime);
            TimeSpan EndTime = TimeSpan.Parse(endTime);
            DateTime UseDate = DateTime.Parse(useDate);
            DateTime sTime = new DateTime(UseDate.Year, UseDate.Month, UseDate.Day, StartTime.Hours,
                StartTime.Minutes, StartTime.Seconds);
            DateTime eTime = new DateTime(UseDate.Year, UseDate.Month, UseDate.Day, EndTime.Hours,
                EndTime.Minutes, EndTime.Seconds);
            var userId = User.Identity.GetUserId();
            var _orderService = this.Service<IOrderService>();
            int pType = Int32.Parse(paidType);
            Order result = _orderService.CreateOrder(userId, fieldId, sTime, eTime, note, price, pType);
            if (result != null)
            {
                return Json(new
                {
                    success = true
                });
            }else
            {
                return Json(new
                {
                    success = false
                });
            }
        }
    }
}