using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IFieldScheduleService
    {
        #region Code from here
        bool checkScheduleTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime playDate);

        bool checkMaintainTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime startDate, DateTime endDate);
        IQueryable<FieldSchedule> GetFieldSchedule(JQueryDataTableParamModel request, out int totalRecord, int placeId);

        #endregion

        void test();
    }
    public partial class FieldScheduleService : IFieldScheduleService
    {
        #region Code from here

        public bool checkMaintainTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime startDate, DateTime endDate)
        {
            IEnumerable<FieldSchedule> schedules = this.GetActive(p => p.FieldId == fieldId).OrderBy(p => p.StartTime).ToList();
            bool isValid = true;
            DateTime sTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hours,
                startTime.Minutes, startTime.Seconds);
            DateTime eTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hours,
                endTime.Minutes, endTime.Seconds);
            foreach (var schedule in schedules)
            {
                if ((schedule.StartTime >= sTime && schedule.EndTime <= eTime) || (schedule.StartTime < sTime &&
                    schedule.EndTime > sTime) || (schedule.EndTime > eTime && schedule.StartTime < eTime))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkScheduleTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime useDate)
        {
            IEnumerable<FieldSchedule> schedules = this.GetActive(p => p.FieldId == fieldId).OrderBy(p => p.StartTime).ToList();
            bool isValid = true;
            foreach (var schedule in schedules)
            {
                if (DateTime.Compare(useDate, schedule.StartTime.Date) == 0)
                {
                    if ((startTime >= schedule.StartTime.TimeOfDay && startTime < schedule.EndTime.TimeOfDay) ||
                                        (endTime > schedule.StartTime.TimeOfDay && endTime <= schedule.EndTime.TimeOfDay) ||
                                        (startTime <= schedule.StartTime.TimeOfDay && endTime >= schedule.EndTime.TimeOfDay))
                    {
                        isValid = false;
                        break;
                    }
                }

            }
            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IQueryable<FieldSchedule> GetFieldSchedule(JQueryDataTableParamModel request, out int totalRecord, int placeId)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive(f => f.Field.PlaceId == placeId);

            var list = list1.Where(
                u => filter == null ||
                u.Field.Name.ToLower().Contains(filter.ToLower())
                );

            totalRecord = list.Count();
            var result = list.OrderBy(u => u.Field.Name)
                .Skip(request.iDisplayStart)
                             .Take(request.iDisplayLength);
            return result;
        }


        #endregion

        public void test()
        {

        }
    }
}