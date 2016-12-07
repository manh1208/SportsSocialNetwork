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
                var user = service.GetActive(u => u.Email.ToLower().Equals(username.ToLower())).FirstOrDefault();
                if (user != null)
                {
                    result.Succeed = true;
                }
                else
                {
                    result.Succeed = false;
                    result.AddError("username", "Email không tồn tại trong hệ thống");
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
                    var user = userService.GetActive(u => u.Email.ToLower().Equals(schedule.UserName.ToLower())).FirstOrDefault();
                    if (user != null)
                    {
                       
                        schedule.UserId = user.Id;
                    }else
                    {
                        result.Succeed = false;
                        result.AddError("Update", "Email không tồn tại trong hệ thống. Vui lòng thử lại");
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

        public ActionResult GetOrderBySport(int sportId)
        {
            var result = new AjaxOperationResult<List<FieldScheduleViewModel>>();
            var userId = User.Identity.GetUserId();
            var fieldScheduleService = this.Service<IFieldScheduleService>();
            DateTime today = DateTime.Now;
            var orderList = fieldScheduleService.GetActive(p => p.Field.FieldType.SportId == sportId &&
            p.EndDate >= today && p.UserId == userId && p.Type == (int)FieldScheduleStatus.Booked).ToList();
            List<FieldScheduleViewModel> resultList = new List<FieldScheduleViewModel>();
            foreach(var item in orderList)
            {
                FieldScheduleViewModel model = new FieldScheduleViewModel(item);
                model.PlaceId = item.Field.PlaceId;
                model.FieldName = item.Field.Name;
                model.PlaceName = item.Field.Place.Name;
                model.StartTimeString = model.StartTime.Hours.ToString("00") + ":" + model.StartTime.Minutes.ToString("00");
                model.EndTimeString = model.EndTime.Hours.ToString("00") + ":" + model.EndTime.Minutes.ToString("00");
                var bits = new bool[8];
                for (var i = 7; i >= 0; i--)
                {
                    bits[i] = (model.AvailableDay & (1 << i)) != 0;
                }
                var dayOfWeek = "";
                List<int> days = new List<int>();
                if (bits[1])
                {
                    dayOfWeek += "CN ";
                    days.Add(1);
                }
                if (bits[2])
                {
                    dayOfWeek += "T2 ";
                    days.Add(2);
                }
                if (bits[3])
                {
                    dayOfWeek += "T3 ";
                    days.Add(3);
                }
                if (bits[4])
                {
                    dayOfWeek += "T4 ";
                    days.Add(4);
                }
                if (bits[5])
                {
                    dayOfWeek += "T5 ";
                    days.Add(5);
                }
                if (bits[6])
                {
                    dayOfWeek += "T6 ";
                    days.Add(6);
                }
                if (bits[7])
                {
                    dayOfWeek += "T7 ";
                    days.Add(7);
                }
                model.availableDayOfWeek = dayOfWeek;
                model.DayOfWeek = days;
                resultList.Add(model);
            }
            result.Succeed = true;
            result.AdditionalData = resultList;
            return Json(result);
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
                    updateSchedule.UserName = schedule.AspNetUser.Email;
                }
            }
            //ViewBag.startDay = startDay;
            //ViewBag.endDay = endDay;
            //ViewBag.startTime = startTime;
            //ViewBag.endTime = endTime;
            return this.PartialView(updateSchedule);
        }

        public ActionResult GetData(JQueryDataTableParamModel param)
        {
            var _orderService = this.Service<IFieldScheduleService>();
            var userId = User.Identity.GetUserId();
            var orderList = _orderService.GetActive(p => p.UserId == userId).OrderByDescending(p => p.Id);
            IEnumerable <FieldSchedule> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = orderList.Where(
                    o => (o.Field.Name != null && o.Field.Name.ToLower().Contains(param.sSearch.ToLower()))
                    );

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

                case 0:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.Field.Name)
                        : filteredListItems.OrderByDescending(o => o.Field.Name);
                    break;
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.StartTime)
                        : filteredListItems.OrderByDescending(o => o.StartTime);
                    break;
                case 3:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.EndDate)
                        : filteredListItems.OrderByDescending(o => o.EndDate);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = displayedList.Select(o => new IConvertible[]{
                o.Id,
                o.Field.Name,
                o.StartTime.Hours.ToString("00")+":"+o.StartTime.Minutes.ToString("00"),
                o.StartDate.ToString("dd/MM/yyyy"),
                o.EndTime.Hours.ToString("00")+":"+o.EndTime.Minutes.ToString("00"),
                o.EndDate.ToString("dd/MM/yyyy"),
                o.AvailableDay
            }.ToArray());

            return Json(new
            {
                param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = filteredListItems.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

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
                    var user = userService.GetActive(u => u.Email.ToLower().Equals(schedule.UserName.ToLower())).FirstOrDefault();
                    if (user != null)
                    {

                        schedule.UserId = user.Id;
                    }
                    else
                    {
                        result.Succeed = false;
                        result.AddError("Update", "Email không tồn tại trong hệ thống. Vui lòng thử lại");
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

        [HttpPost]
        public ActionResult GetDays(string startDate, string endDate)
        {
            
            var result = new AjaxOperationResult<List<Select2>>();
            try { 
            DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int index = 0;
            List<Select2> days = new List<Select2>();
            while (start <= end && (index++)<7)
            {
                int dayOfWeek = ((int)start.DayOfWeek) + 1;
                switch (dayOfWeek)
                {
                    case 1:
                        days.Add(new Select2
                        {
                            id = dayOfWeek.ToString(),
                           text= "Chủ nhật"
                        });
                        break;
                    case 2:
                        days.Add(new Select2
                        {
                            id = dayOfWeek.ToString(),
                            text = "Thứ hai"
                        });
                        break;
                    case 3:
                        days.Add(new Select2
                        {
                            id = dayOfWeek.ToString(),
                            text = "Thứ ba"
                        });
                        break;
                    case 4:
                        days.Add(new Select2
                        {
                            id = dayOfWeek.ToString(),
                            text = "Thứ tư"
                        });
                        break;
                    case 5:
                        days.Add(new Select2
                        {
                            id = dayOfWeek.ToString(),
                            text = "Thứ năm"
                        });
                        break;
                    case 6:
                        days.Add(new Select2
                        {
                            id = dayOfWeek.ToString(),
                            text = "Thứ sáu"
                        });
                        break;
                    case 7:
                        days.Add(new Select2
                        {
                            id = dayOfWeek.ToString(),
                            text = "Thứ bảy"
                        });
                        break;

                }
                start = start.AddDays(1);
            }
                var dayenum = days.AsEnumerable().OrderBy(u=>u.id);
                
           
            result.AdditionalData = dayenum.ToList();
            }
            catch (Exception)
            {
                result.Succeed = false;
            }
            return Json(result);
        }
    }

    public class Select2
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}