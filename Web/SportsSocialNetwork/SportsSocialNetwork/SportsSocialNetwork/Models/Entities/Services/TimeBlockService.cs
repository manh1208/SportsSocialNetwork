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
        void saveTimeBLock(int fieldId, List<string> timeBLock);
        bool deleteTimeBlock(string timeblock);
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

        public void saveTimeBLock(int fieldId, List<string> timeBlock)
        {
            char delimiter = '_';
            for(int i = 0; i < timeBlock.Count; i++)
            {
                string orStr = timeBlock[i];
                string[] tokens = orStr.Split(delimiter);
                TimeSpan startTime = TimeSpan.Parse(tokens[0]);
                TimeSpan endTime = TimeSpan.Parse(tokens[1]);
                double price = Double.Parse(tokens[2]);
                if(tokens.Length > 3)
                {
                    if (!String.IsNullOrEmpty(tokens[3]))
                    {
                        int id = Int32.Parse(tokens[3]);
                        TimeBlock sTimeBlock = this.FirstOrDefaultActive(t => t.Id == id);
                        if (sTimeBlock != null)
                        {
                            sTimeBlock.StartTime = startTime;
                            sTimeBlock.EndTime = endTime;
                            sTimeBlock.Price = price;
                            this.Update(sTimeBlock);
                            this.Save();
                        }
                    }
                }
                else
                {
                    TimeBlock newTB = new TimeBlock();
                    newTB.FieldId = fieldId;
                    newTB.StartTime = startTime;
                    newTB.EndTime = endTime;
                    newTB.Price = price;
                    this.Create(newTB);
                    this.Save();
                }
            }
        }

        public bool deleteTimeBlock(string timeblock)
        {
            char delimiter = '_';
            string[] tokens = timeblock.Split(delimiter);
            if(tokens.Length > 3)
            {
                if (!String.IsNullOrEmpty(tokens[3]))
                {
                    int id = Int32.Parse(tokens[3]);
                    TimeBlock sTimeBlock = this.FirstOrDefaultActive(t => t.Id == id);
                    if (sTimeBlock != null)
                    {
                        this.Delete(sTimeBlock);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}