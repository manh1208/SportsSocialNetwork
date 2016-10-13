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


        #endregion

        void test();
    }
    public partial class FieldScheduleService : IFieldScheduleService
    {
        #region Code from here
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


        #endregion

        public void test()
        {

        }
    }
}