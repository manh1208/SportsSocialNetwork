using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface ITimeBlockService
    {
        double calPrice(int fieldId, TimeSpan startTime, TimeSpan endTime);
        bool checkTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime);
    }
    public partial class TimeBlockService : ITimeBlockService
    {
        public bool checkTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime)
        {
            return true;
        }
        public double calPrice(int fieldId, TimeSpan startTime, TimeSpan endTime)
        {
            IEnumerable<TimeBlock> timeBlocks = this.Get(p => p.FieldId == fieldId).ToList();
            double price = 0;
            foreach (var block in timeBlocks)
            {
                if (startTime >= block.StartTime && startTime <= block.EndTime)
                {
                    if (endTime <= block.EndTime)
                    {
                        double totalHours = (endTime.Hours - startTime.Hours);
                        double totalMins = (endTime.Minutes - startTime.Minutes);
                        double totalTime = totalHours + totalMins / 60;
                        price = totalTime * block.Price;
                        break;
                    }
                    else
                    {
                        double hoursInSlot = (block.EndTime.Hours - startTime.Hours);
                        double minsInSlot = (block.EndTime.Minutes - startTime.Minutes);
                        double timeInSlot = hoursInSlot + minsInSlot / 60;
                        price += timeInSlot * block.Price;
                    }
                }
                if (startTime <= block.StartTime && endTime >= block.EndTime)
                {
                    double hoursInSlot = (block.EndTime.Hours - block.StartTime.Hours);
                    double minsInSlot = (block.EndTime.Minutes - block.StartTime.Minutes);
                    double timeInSlot = hoursInSlot + minsInSlot / 60;
                    price += timeInSlot * block.Price;
                }
                if (endTime <= block.EndTime)
                {
                    double hoursInSlot = (endTime.Hours - block.StartTime.Hours);
                    double minsInSlot = (endTime.Minutes - block.StartTime.Minutes);
                    double timeInSlot = hoursInSlot + minsInSlot / 60;
                    price += timeInSlot * block.Price;
                    break;
                }
            }
            return price;
        }
    }
}