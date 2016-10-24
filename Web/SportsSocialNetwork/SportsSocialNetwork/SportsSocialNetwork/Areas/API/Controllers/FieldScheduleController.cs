using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class FieldScheduleController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult CreateFieldSchedule(FieldScheduleCreateViewModel model)
        {
            var scheduleService = this.Service<IFieldScheduleService>();

            ResponseModel<FieldScheduleViewModel> response = null;

            DateTime startTime = new DateTime();

            DateTime endTime = new DateTime();

            try
            {
                startTime = DateTime.ParseExact(model.StartTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                endTime = DateTime.ParseExact(model.EndTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {
                response = ResponseModel<FieldScheduleViewModel>.CreateErrorResponse("Tạo lịch thất bại", "Lỗi định dạng thời gian");

                return Json(response);
            }
            try
            {
                if (scheduleService.checkTimeValidInFieldSchedule(model.FieldId, startTime.TimeOfDay,endTime.TimeOfDay, startTime, endTime)) {
                    FieldSchedule schedule = new FieldSchedule();
                    schedule.FieldId = model.FieldId;
                    schedule.Type = model.Type;
                    schedule.StartTime = startTime;
                    schedule.EndTime = endTime;
                    schedule.Description = model.Description;
                    scheduleService.Create(schedule);
                    scheduleService.Save();

                    FieldScheduleViewModel result = Mapper.Map<FieldScheduleViewModel>(schedule);

                    PrepareFieldScheduleViewModel(result);

                    response = new ResponseModel<FieldScheduleViewModel>(true, "Lịch đã được tạo.", null, result);

                }
                else
                {
                    response = ResponseModel<FieldScheduleViewModel>.CreateErrorResponse("Tạo lịch thất bại", "Khung giờ đã tồn tại");
                }

            }
            catch (Exception)
            {
                response = ResponseModel<FieldScheduleViewModel>.CreateErrorResponse("Tạo lịch thất bại", systemError);
            }
            return Json(response);

        }

        private void PrepareFieldScheduleViewModel(FieldScheduleViewModel schedule) {
            schedule.EndTimeString = schedule.EndTime.ToString("dd/MM/yyyy HH:mm:ss");

            schedule.StartTimeString = schedule.StartTime.ToString("dd/MM/yyyy HH:mm:ss");

            schedule.TypeString = Utils.GetEnumDescription((FieldScheduleStatus)schedule.Type);
        }
    }
}