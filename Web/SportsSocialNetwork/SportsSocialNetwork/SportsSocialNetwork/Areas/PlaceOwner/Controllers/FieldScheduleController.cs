﻿using SkyWeb.DatVM.Mvc;
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

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    public class FieldScheduleController : BaseController
    {
        // GET: PlaceOwner/FieldSchedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexList(JQueryDataTableParamModel request)
        {
            //var fieldService = this.Service<IFieldService>();
            //var fieldScheduleService = this.Service<IFieldScheduleService>();
            //Field field = fieldService.FirstOrDefault();
            //FieldSchedule fieldSchedule = fieldScheduleService.FirstOrDefault(x => x.FieldId == field.Id);
            //var totalRecord = 0;
            //var count = 1;

            //var results = fieldScheduleService.GetFieldSchedule(request, out totalRecord)
            //    .AsEnumerable()
            //    .Select(a => new IConvertible[] {
            //            count++,
            //            a.Field.Name,
            //            a.StartTime.ToString(),
            //            a.EndTime.ToString(),
            //            a.Type,
            //            a.Id,
            //    }).ToArray();

            //var model = new
            //{
            //    draw = request.sEcho,
            //    data = results,
            //    recordsFiltered = totalRecord,
            //    recordsTotal = totalRecord
            //};
            //return Json(model);
            return Json(new { });
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
            }
            
            return this.PartialView(model);
        }


        [HttpGet]
        public ActionResult Create()
        {
            string userID = User.Identity.GetUserId();
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            List<Place> listPlace = new List<Place>();
            List<Field> listField = new List<Field>();
            listPlace = _placeService.GetActive(p => p.UserId == userID).ToList();
            foreach (var item in listPlace)
            {
                Field field = new Field();
                listField = _fieldService.GetActive(x => x.PlaceId == item.Id).ToList();
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
            return this.PartialView();
        }

       

        [HttpPost]
        public ActionResult Create(FieldSchedule schedule)
        {
            DateTime _startDay = DateTime.Parse(Request["StartDay"]);
            DateTime _endDay = DateTime.Parse(Request["EndDay"]);
            TimeSpan _startTime = TimeSpan.Parse(Request["StartTime"]);
            TimeSpan _endTime = TimeSpan.Parse(Request["EndTime"]);

            DateTime startTime = new DateTime(_startDay.Date.Year, _startDay.Date.Month, _startDay.Date.Day, _startTime.Hours, _startTime.Minutes, _startTime.Seconds);
            DateTime endTime = new DateTime(_endDay.Date.Year, _endDay.Date.Month, _endDay.Date.Day, _endTime.Hours, _endTime.Minutes, _endTime.Seconds);
            var result = new AjaxOperationResult();
            try
            {               
                var typeService = this.Service<IFieldTypeService>();
                var fieldService = this.Service<IFieldService>();
                var scheduleService = this.Service<IFieldScheduleService>();                             
                FieldSchedule fs = new FieldSchedule();
                fs.FieldId = schedule.FieldId;
                fs.Type = schedule.Type;
                fs.StartTime = startTime;
                fs.EndTime = endTime;
                fs.Description = schedule.Description;
                scheduleService.Create(fs);
                scheduleService.Save();
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.Succeed = false;
            }
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

        public ActionResult Update(int id)
        {
            string userID = User.Identity.GetUserId();
            var scheduleService = this.Service<IFieldScheduleService>();
            FieldSchedule schedule = scheduleService.Get(id);
            FieldScheduleViewModel updateSchedule;

            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            List<Place> listPlace = new List<Place>();
            List<Field> listField = new List<Field>();
            listPlace = _placeService.GetActive(p => p.UserId == userID).ToList();
            foreach (var item in listPlace)
            {
                Field field = new Field();
                listField = _fieldService.GetActive(x => x.PlaceId == item.Id).ToList();
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

            DateTime startDay = new DateTime(schedule.StartTime.Year, schedule.StartTime.Month, schedule.StartTime.Day);
            DateTime endDay = new DateTime(schedule.EndTime.Year, schedule.EndTime.Month, schedule.EndTime.Day);
            TimeSpan startTime = new TimeSpan(schedule.StartTime.Hour, schedule.StartTime.Minute, schedule.StartTime.Second);
            TimeSpan endTime = new TimeSpan(schedule.EndTime.Hour, schedule.EndTime.Minute, schedule.EndTime.Second);
         
            if (schedule == null)
            {
                return this.IdNotFound();
            }
            else
            {
                updateSchedule = Mapper.Map<FieldScheduleViewModel>(schedule);
            }
            ViewBag.startDay = startDay;
            ViewBag.endDay = endDay;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            return this.PartialView(updateSchedule);
        }

        [HttpPost]
        public ActionResult Update(FieldScheduleViewModel model)
        {
            var result = new AjaxOperationResult();
            try
            {
                var scheduleService = this.Service<IFieldScheduleService>();
                FieldSchedule schedule = scheduleService.Get(model.Id);
                schedule.FieldId = model.FieldId;
                schedule.StartTime = model.StartTime;
                schedule.EndTime = model.EndTime;
                schedule.Type = model.Type;
                schedule.Description = model.Description;
                scheduleService.Update(schedule);
                scheduleService.Save();
                result.Succeed = true;
            }
            catch (Exception)
            {
                result.Succeed = false;
            }           
            return Json(result);
        }
    }
}