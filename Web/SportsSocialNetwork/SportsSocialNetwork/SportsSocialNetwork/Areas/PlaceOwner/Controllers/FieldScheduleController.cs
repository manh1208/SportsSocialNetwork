using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.PlaceOwner.Models.ViewModels;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System.Web.Routing;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    public class FieldScheduleController : BaseController
    {
        // GET: PlaceOwner/FieldSchedule
        public ActionResult Index(int? id)
        {
            ViewBag.placeID = id.Value;
            return View();
        }

        public ActionResult List(int? id)
        {
            ViewBag.placeID = id.Value;
            return View();
        }

        public ActionResult IndexList(JQueryDataTableParamModel request)
        {
            int placeID = Int32.Parse(Request["placeID"]);
            //var fieldService = this.Service<IFieldService>();
            var fieldScheduleService = this.Service<IFieldScheduleService>();
            //List<Field> listField = fieldService.Get(f => f.PlaceId == placeID).ToList();
            //List<FieldSchedule> listFS = new List<FieldSchedule>();
            //foreach (var item in listField)
            //{
            //    listFS.AddRange(fieldScheduleService.Get(f => f.FieldId == item.Id).ToList());
            //}
            //Field field = fieldService.FirstOrDefault();
            //FieldSchedule fieldSchedule = fieldScheduleService.FirstOrDefault(x => x.FieldId == field.Id);
            var totalRecord = 0;
            var count = 1;

            var results = fieldScheduleService.GetFieldSchedule(request, out totalRecord, placeID)
                .AsEnumerable()
                .Select(a => new IConvertible[] {
                        count++,
                        a.Field.Name,
                        a.StartDate.ToString("dd/MM/yyyy"),
                        a.EndDate.ToString("dd/MM/yyyy"),

                        a.Type,
                        a.Id,
                }).ToArray();

            var model = new
            {
                draw = request.sEcho,
                data = results,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord
            };
            return Json(model);
            //return Json(new { });
        }

        public ActionResult Detail(int id)
        {
            var model = new FieldScheduleDetailViewModel();

            var service = this.Service<IFieldScheduleService>();
            var schedule = service.Get(id);
            if (schedule == null)
            {
                return this.IdNotFound();
            }
            else
            {
                model = Mapper.Map<FieldScheduleDetailViewModel>(schedule);
                int repeat = model.AvailableDay;
                int i = 0;
                string s = "";
                while (repeat > 0)
                {

                    if ((repeat & 1) == 1)
                    {
                        switch (i)
                        {
                            case 1:
                                s += "Chủ nhật - ";
                                break;
                            case 2:
                                s += "Thứ hai - ";
                                break;
                            case 3:
                                s += "Thứ ba - ";
                                break;
                            case 4:
                                s += "Thứ tư - ";
                                break;
                            case 5:
                                s += "Thứ năm - ";
                                break;
                            case 6:
                                s += "Thứ sáu - ";
                                break;
                            case 7:
                                s += "Thứ bảy - ";
                                break;

                        }

                    }

                    i++;
                    repeat >>= 1;
                }
                s = s.Substring(0, s.Length - 3);
                model.AvailableDayStr = s;
            }

            return this.PartialView(model);
        }


        [HttpGet]
        public ActionResult Create(int placeId)
        {
            string userID = User.Identity.GetUserId();
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            List<Place> listPlace = new List<Place>();
            List<Field> listField = new List<Field>();
            listPlace = _placeService.GetActive(p => p.UserId == userID).ToList();
            foreach (var item in listPlace)
            {
                if (item.Id == placeId)
                {
                    listField.AddRange(_fieldService.GetActive(x => x.PlaceId == item.Id).ToList());
                }

            }
            List<SelectListItem> selectListField = new List<SelectListItem>();
            foreach (var item in listField)
            {
                selectListField.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }

            List<SelectListItem> scheduleType = new List<SelectListItem>();
            foreach (FieldScheduleStatus item in Enum.GetValues(typeof(FieldScheduleStatus)))
            {
                scheduleType.Add(new SelectListItem { Value = Convert.ToString((int)item), Text = Utils.GetEnumDescription(item) });
            }
            ViewBag.scheduleType = scheduleType;
            ViewBag.selectListField = selectListField;
            ViewBag.placeId = placeId;

            return this.PartialView();
        }


        [HttpPost]

        public ActionResult CheckUsername(string username)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IAspNetUserService>();
            if (username != null && username.Length > 0)
            {
                var user = service.GetActive(u => u.UserName.ToLower().Equals(username.ToLower())).FirstOrDefault();
                if (user != null)
                {
                    result.Succeed = true;
                }
                else
                {
                    result.Succeed = false;
                    result.AddError("username", "Tên tài khoản không có thật");
                }
            }
            else
            {
                result.Succeed = true;
            }

            return Json(result);

        }


        [HttpPost]
        public ActionResult Create(CreateFieldScheduleViewModel schedule)
        {
            var result = new AjaxOperationResult();

            try
            {

                schedule.StartDate = DateTime.ParseExact(schedule.StartDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                schedule.EndDate = DateTime.ParseExact(schedule.EndDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                schedule.StartTime = TimeSpan.Parse(schedule.StartTimeStr);
                schedule.EndTime = TimeSpan.Parse(schedule.EndTimeStr);

                string[] days = schedule.Days.Split(',');
                int repeatDay = 0;
                for (int i = 0; i < days.Length; i++)
                {
                    if (days[i].Length > 0)
                    {
                        int mark = 1 << Int16.Parse(days[i]);
                        repeatDay = repeatDay | mark;
                    }
                }
                schedule.AvailableDay = repeatDay;
                var userService = this.Service<IAspNetUserService>();
                if (schedule.UserName != null && schedule.UserName.Length > 0)
                {
                    var user = userService.GetActive(u => u.UserName.ToLower().Equals(schedule.UserName.ToLower())).FirstOrDefault();
                    if (user != null)
                    {
                       
                        schedule.UserId = user.Id;
                    }else
                    {
                        result.Succeed = false;
                        result.AddError("Update", "Tên tài khoản không đúng. Vui lòng thử lại");
                        return Json(result);
                    }
                }
                schedule.Active = true;
                
                if (checkValid(schedule))
                {
                    var entity = schedule.ToEntity();
                    var scheduleService = this.Service<IFieldScheduleService>();
                    scheduleService.Create(entity);
                    result.Succeed = true;
                    return Json(result);
                }
                else
                {
                    result.Succeed = false;
                    result.AddError("Update", "Thời gian đặt bị trùng lịch. Vui lòng chọn thời gian khác");
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                result.Succeed = false;
                result.AddError("Update", "Vui lòng nhập đầy đủ thông tin");
                return Json(result);
            }
            //DateTime startTime = new DateTime(_startDay.Date.Year, _startDay.Date.Month, _startDay.Date.Day, _startTime.Hours, _startTime.Minutes, _startTime.Seconds);
            //DateTime endTime = new DateTime(_endDay.Date.Year, _endDay.Date.Month, _endDay.Date.Day, _endTime.Hours, _endTime.Minutes, _endTime.Seconds);
            //var result = new AjaxOperationResult();




           
        }

        [HttpPost]
        public ActionResult Deactive(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IFieldScheduleService>();
            var schedule = service.Get(id);
            if (schedule == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    service.Deactivate(schedule);
                    result.Succeed = true;
                }
                catch (Exception)
                {
                    result.Succeed = false;
                }
            }
            return Json(result);
        }

        public ActionResult Update(int id, int placeId)
        {
            string userID = User.Identity.GetUserId();
            var scheduleService = this.Service<IFieldScheduleService>();
            FieldSchedule schedule = scheduleService.Get(id);
            CreateFieldScheduleViewModel updateSchedule;

            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            List<Place> listPlace = new List<Place>();
            List<Field> listField = new List<Field>();
            listPlace = _placeService.GetActive(p => p.UserId == userID).ToList();
            foreach (var item in listPlace)
            {
                if (item.Id == placeId)
                {
                    listField.AddRange(_fieldService.GetActive(x => x.PlaceId == item.Id).ToList());
                }
            }
            List<SelectListItem> selectListField = new List<SelectListItem>();
            foreach (var item in listField)
            {
                selectListField.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }

            List<SelectListItem> scheduleType = new List<SelectListItem>();
            foreach (FieldScheduleStatus item in Enum.GetValues(typeof(FieldScheduleStatus)))
            {
                scheduleType.Add(new SelectListItem { Value = Convert.ToString((int)item), Text = Utils.GetEnumDescription(item) });
            }
            ViewBag.scheduleType = scheduleType;
            ViewBag.selectListField = selectListField;

            //DateTime startDay = new DateTime(schedule.StartTime.Year, schedule.StartTime.Month, schedule.StartTime.Day);
            //DateTime endDay = new DateTime(schedule.EndTime.Year, schedule.EndTime.Month, schedule.EndTime.Day);
            //TimeSpan startTime = new TimeSpan(schedule.StartTime.Hour, schedule.StartTime.Minute, schedule.StartTime.Second);
            //TimeSpan endTime = new TimeSpan(schedule.EndTime.Hour, schedule.EndTime.Minute, schedule.EndTime.Second);

            if (schedule == null)
            {
                return this.IdNotFound();
            }
            else
            {
                updateSchedule = Mapper.Map<CreateFieldScheduleViewModel>(schedule);
                updateSchedule.StartDateStr = updateSchedule.StartDate.ToString("dd/MM/yyyy");
                updateSchedule.EndDateStr = updateSchedule.EndDate.ToString("dd/MM/yyyy");
                string start = updateSchedule.StartTime.ToString();
                updateSchedule.StartTimeStr = start.Substring(0, start.Length - 3);
                string end = updateSchedule.EndTime.ToString();
                updateSchedule.EndTimeStr = end.Substring(0, end.Length - 3);
                int repeat = updateSchedule.AvailableDay;
                int i = 0;
                string s = "";
                while (repeat > 0)
                {

                    if ((repeat & 1) == 1)
                    {
                        s += "," + i;
                    }

                    i++;
                    repeat >>= 1;
                }
                updateSchedule.Days = s;
                updateSchedule.PlaceId = placeId;
                if (schedule.UserId != null)
                {
                    updateSchedule.UserName = schedule.AspNetUser.UserName;
                }
            }
            //ViewBag.startDay = startDay;
            //ViewBag.endDay = endDay;
            //ViewBag.startTime = startTime;
            //ViewBag.endTime = endTime;
            return this.PartialView(updateSchedule);
        }

        [HttpPost]
        public ActionResult Update(CreateFieldScheduleViewModel schedule)
        {
            var result = new AjaxOperationResult();

            try
            {

                schedule.StartDate = DateTime.ParseExact(schedule.StartDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                schedule.EndDate = DateTime.ParseExact(schedule.EndDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                schedule.StartTime = TimeSpan.Parse(schedule.StartTimeStr);
                schedule.EndTime = TimeSpan.Parse(schedule.EndTimeStr);
                string[] days = schedule.Days.Split(',');
                int repeatDay = 0;
                for (int i = 0; i < days.Length; i++)
                {
                    if (days[i].Length > 0)
                    {
                        int mark = 1 << Int16.Parse(days[i]);
                        repeatDay = repeatDay | mark;
                    }
                }
                schedule.AvailableDay = repeatDay;
                var userService = this.Service<IAspNetUserService>();
                if (schedule.UserName != null && schedule.UserName.Length > 0)
                {
                    var user = userService.GetActive(u => u.UserName.ToLower().Equals(schedule.UserName.ToLower())).FirstOrDefault();
                    if (user != null)
                    {

                        schedule.UserId = user.Id;
                    }
                    else
                    {
                        result.Succeed = false;
                        result.AddError("Update", "Tên tài khoản không đúng. Vui lòng thử lại");
                        return Json(result);
                    }
                }
                schedule.Active = true;
                var scheduleService = this.Service<IFieldScheduleService>();
                var entity = scheduleService.Get(schedule.Id);
               
                if (checkValid(schedule))
                {
                    schedule.CopyToEntity(entity);
                    scheduleService.Update(entity);
                    result.Succeed = true;
                }
                else
                {
                    result.Succeed = false;
                    result.AddError("Update", "Thời gian đặt bị trùng lịch. Vui lòng chọn thời gian khác");
                }
            }
            catch (Exception e)
            {
                result.Succeed = false;
                result.AddError("Update", "Vui lòng nhập đầy đủ thông tin");
            }
            //DateTime startTime = new DateTime(_startDay.Date.Year, _startDay.Date.Month, _startDay.Date.Day, _startTime.Hours, _startTime.Minutes, _startTime.Seconds);
            //DateTime endTime = new DateTime(_endDay.Date.Year, _endDay.Date.Month, _endDay.Date.Day, _endTime.Hours, _endTime.Minutes, _endTime.Seconds);
            //var result = new AjaxOperationResult();




            return Json(result);
        }

        public bool checkValid(CreateFieldScheduleViewModel schedule)
        {
            var _fieldScheduleService = this.Service<IFieldScheduleService>();
            string tit = Utils.GetEnumDescription((FieldScheduleStatus)schedule.Type);
            DateTime start = schedule.StartDate;
            DateTime end = schedule.EndDate;
            while (start <= end)
            {
                int dayOfWeek = ((int)start.DayOfWeek) + 1;
                int bitAtPosititon = (schedule.AvailableDay >> dayOfWeek) & 1;
                if (bitAtPosititon == 1)
                {
                    var result = _fieldScheduleService.checkTimeValidInFieldSchedule(schedule.Id,schedule.FieldId, schedule.StartTime, schedule.EndTime, start, start);
                    if (!result)
                    {
                        return false;
                    }
                }
                start = start.AddDays(1);
            }
            return true;
        }

        [HttpPost]
        public string checkDTValid(int fieldId, string startDay, string endDay, string startTime, string endTime)
        {
            //var _fieldScheduleService = this.Service<IFieldScheduleService>();
            //DateTime _startDay = DateTime.ParseExact(startDay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime _endDay = DateTime.ParseExact(endDay, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //TimeSpan _startTime = TimeSpan.Parse(startTime);
            //TimeSpan _endTime = TimeSpan.Parse(endTime);
            //if (_fieldScheduleService.checkTimeValidInFieldSchedule(fieldId,_startTime,_endTime,_startDay,_endDay))
            //{
            //    return "valid";
            //}
            //else
            //{
            //    return "invalid";
            //}
            return "valid";
        }
    }
}