using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsSocialNetwork.Models.Entities.Services;
using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.ViewModels;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Identity;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
    public class FieldController : BaseController
    {
        // GET: Field
        public ActionResult Index(int? id)
        {
            var _fieldService = this.Service<IFieldService>();
            var _fieldImageService = this.Service<IFieldImageService>();
            var _timeBlockService = this.Service<ITimeBlockService>();
            var entity = _fieldService.FirstOrDefaultActive(p => p.Id == id.Value);
            var fields = new FieldViewModel(entity);
            List<FieldImage> fieldImages = _fieldImageService.GetActive(p => p.FieldId == id.Value).ToList();
            if (fieldImages.Count > 0)
            {
                ViewBag.fieldImages = fieldImages;
            }
            List<TimeBlock> timeBlocks = _timeBlockService.GetActive(p => p.FieldId == id.Value).ToList();
            if(timeBlocks.Count > 0)
            {
                ViewBag.timeBlocks = timeBlocks;
            }
            return View(fields);
        }

        //public ActionResult GetSchedule(int? id)
        //{
        //    var _orderService = this.Service<IOrderService>();
        //    List<Order> orders = _orderService.GetActive(p => p.FieldId == id && p.Status != 4).ToList();
        //    var _fieldScheduleService = this.Service<IFieldScheduleService>();
        //    List<FieldSchedule> schedules = _fieldScheduleService.GetActive(p => p.FieldId == id).ToList();
        //    List<OrderViewModel> orderList = Mapper.Map<List<OrderViewModel>>(orders);
        //    var s = schedules.Select(m => new
        //    {
        //        title = Utils.GetEnumDescription((FieldScheduleStatus)m.Type),
        //        start = m.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
        //        end = m.EndTime.ToString("yyyy-MM-ddTHH:mm:ss")
        //    }).ToList();

        //    var o = orders.Select(m => new
        //    {
        //        title = "Đã được đặt",
        //        start = m.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
        //        end = m.EndTime.ToString("yyyy-MM-ddTHH:mm:ss")
        //    }).ToList();
        //    var temp = s.Concat(o);
        //    return Json(temp, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetOrderCalendar(int? id)
        {
            var _orderService = this.Service<IOrderService>();
            List<Order> orders = _orderService.GetActive(p => p.FieldId == id && p.Status!=4).ToList();
            List<OrderViewModel> orderList = Mapper.Map<List<OrderViewModel>>(orders);
            return Json(orderList.Select(f => new { title = "Đã được đặt",start=f.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),end=f.EndTime.ToString("yyyy-MM-ddTHH:mm:ss") }), JsonRequestBehavior.AllowGet );
        }

        public ActionResult GetScheduleCalendar(int? id)
        {
            var _fieldScheduleService = this.Service<IFieldScheduleService>();
            List<FieldSchedule> schedules = _fieldScheduleService.GetActive(p => p.FieldId == id).ToList();
            List<FieldScheduleViewModel> scheduleList = Mapper.Map<List<FieldScheduleViewModel>>(schedules);
            var scheduleCalendar = new List<Calendar>();
            foreach (var schedule in scheduleList)
            {
                string tit = Utils.GetEnumDescription((FieldScheduleStatus)schedule.Type);
                DateTime start = schedule.StartDate;
                DateTime end = schedule.EndDate;
                while (start <= end)
                {
                    int dayOfWeek = ((int)start.DayOfWeek) + 1;
                    int bitAtPosititon = (schedule.AvailableDay >> dayOfWeek) & 1;
                    if (bitAtPosititon == 1)
                    {
                        DateTime startTime = new DateTime(start.Year, start.Month, start.Day, schedule.StartTime.Hours,
                            schedule.StartTime.Minutes, schedule.StartTime.Seconds);
                        DateTime endTime = new DateTime(start.Year, start.Month, start.Day, schedule.EndTime.Hours,
                           schedule.EndTime.Minutes, schedule.EndTime.Seconds);
                        scheduleCalendar.Add(new Calendar
                        {
                            title = tit,
                            start = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                            end = endTime.ToString("yyyy-MM-ddTHH:mm:ss")
                        }
                            );
                    }
                    start = start.AddDays(1);
                }
            }

            return Json(scheduleCalendar, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTimeBlockPrice(int? id)
        {
            var _timeBlockService = this.Service<ITimeBlockService>();
            List<TimeBlock> timeBlocks = _timeBlockService.GetActive(p => p.FieldId == id).ToList();
            return Json(timeBlocks.Select(f => new { block = f.StartTime + " - " + f.EndTime, price = f.Price.ToString("n0") }).ToArray()
                , JsonRequestBehavior.AllowGet);
        }

    }

    public class Calendar
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}