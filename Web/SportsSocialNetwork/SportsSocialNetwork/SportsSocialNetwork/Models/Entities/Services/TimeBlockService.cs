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
            
            bool startTimeValid = false;
            bool endTimeValid = false;
            bool isInGapBetweenTimeBlocks = false;
            IEnumerable<TimeBlock> timeBlocks = this.GetActive(p => p.FieldId == fieldId).OrderBy(p => p.StartTime).ToList();
            TimeSpan tmp = timeBlocks.First().StartTime;
            foreach (var block in timeBlocks)
            {
                if (startTime >= block.StartTime && startTime <= block.EndTime)
                {
                    startTimeValid = true;
                }

                if (endTime >= block.StartTime && endTime <= block.EndTime)
                {
                    endTimeValid = true;
                }
            }
            foreach (var block in timeBlocks)
            {
                if(block.StartTime != tmp)
                {
                    if((tmp>=startTime && tmp < endTime) || (block.StartTime>startTime && block.StartTime <= endTime) || 
                        (tmp <= startTime && block.StartTime >= endTime))
                    {
                        isInGapBetweenTimeBlocks = true;
                    }
                }
                tmp = block.EndTime;

            }
            if (startTimeValid && endTimeValid && !isInGapBetweenTimeBlocks)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public double calPrice(int fieldId, TimeSpan startTime, TimeSpan endTime)
        {
            IEnumerable<TimeBlock> timeBlocks = this.GetActive(p => p.FieldId == fieldId).OrderBy(p => p.StartTime).ToList();
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
                        price += totalTime * block.Price;
                        break;
                    }
                    else
                    {
                        double hoursInSlot = (block.EndTime.Hours - startTime.Hours);
                        double minsInSlot = (block.EndTime.Minutes - startTime.Minutes);
                        double timeInSlot = hoursInSlot + minsInSlot / 60;
                        price += timeInSlot * block.Price;
                        startTime = block.EndTime;
                    }
                }
            }
            return price;
        }
    }
}