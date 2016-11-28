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
        IQueryable<FieldSchedule> GetFieldSchedule(JQueryDataTableParamModel request, out int totalRecord, int placeId);

        bool checkTimeValidInFieldSchedule(int? scheduleId, int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime startDate, DateTime endDate);
        #endregion

        void test();
    }
    public partial class FieldScheduleService : IFieldScheduleService
    {
        #region Code from here

        public bool checkTimeValidInFieldSchedule(int? scheduleId, int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime startDate, DateTime endDate)
        {
            IEnumerable<FieldSchedule> schedules = this.GetActive(p => p.FieldId == fieldId).ToList();
            
            foreach (var schedule in schedules)
            {
                if (scheduleId.HasValue && scheduleId.Value== schedule.Id)
                {
                    continue;
                }
                if(!((schedule.StartDate > startDate && schedule.StartDate > endDate) || 
                    (schedule.EndDate < startDate && schedule.EndDate < endDate)))
                {
                    int dayOfWeek = ((int)startDate.DayOfWeek) +1;
                    int bitAtPosititon = (schedule.AvailableDay >> dayOfWeek) & 1;
                    if (bitAtPosititon == 1)
                    {
                        if (!((schedule.StartTime > startTime && schedule.StartTime >= endTime) ||
                            (schedule.EndTime<=startTime && schedule.EndTime<endTime))){
                            return false;
                        }
                    }
                }
            }

            //DateTime sTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hours,
            //    startTime.Minutes, startTime.Seconds);
            //DateTime eTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hours,
            //    endTime.Minutes, endTime.Seconds);
            //foreach (var schedule in schedules)
            //{
            //    if ((schedule.StartTime > sTime && schedule.StartTime >= eTime) || (schedule.EndTime <= sTime &&
            //        schedule.EndTime < eTime))
            //    {
            //        isValid = true;
            //    }
            //    else
            //    {
            //        isValid = false;
            //        break;
            //    }
            //}
            //if (isValid)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
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